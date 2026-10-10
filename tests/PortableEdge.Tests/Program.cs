using System.Diagnostics;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using Edge_Updater;
using PortableEdge;

if (args.Length == 2 && args[0] == "--ui-smoke")
{
    Exception failure = null;
    var thread = new Thread(() =>
    {
        try
        {
            foreach (string culture in new[] { "en-US", "nl-NL", "de-DE", "ru-RU" })
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
                UiLanguage.UseEnglish();
                Check(CultureInfo.CurrentUICulture.Name == "en-US" && CultureInfo.DefaultThreadCurrentUICulture.Name == "en-US" &&
                    CultureInfo.CurrentCulture.Name == culture, "English UI with regional formatting preserved: " + culture);
                using (var form = new Edge_Updater.Form1())
                {
                    AssertText(form, "button9", "Install all");
                    AssertText(form, "button10", "Quit");
                    AssertText(form, "label10", "Install all x86 and/or x64 versions");
                    AssertText(form, "checkBox1", "Use a separate folder for each version");
                    AssertText(form, "checkBox4", "Ignore version check");
                    AssertText(form, "checkBox5", "Create a desktop shortcut");
                    var expectedMenus = new HashSet<string> { "Tools", "Version info", "Register", "Remove", "Download policy templates" };
                    foreach (string channel in new[] { "", " Stable x86", " Stable x64", " Beta x86", " Beta x64", " Developer x86", " Developer x64", " Canary x86", " Canary x64" })
                        expectedMenus.Add("Edge Chromium" + channel + " as default browser");
                    var menuTexts = form.Controls.OfType<MenuStrip>().SelectMany(menu => MenuTexts(menu.Items)).ToHashSet();
                    Check(menuTexts.SetEquals(expectedMenus), "All updater menus are English: " + culture);
                    Check(form.Controls.Count > 0, "Updater form and resources load: " + culture);
                }
                int launchers = 0;
                foreach (string directory in Directory.GetDirectories(Path.Combine(args[1], "Launcher")))
                {
                    string name = Path.GetFileName(directory);
                    string dll = Path.Combine(directory, "bin", "Debug", "net10.0-windows", name + ".dll");
                    if (!File.Exists(dll)) throw new FileNotFoundException("Build all launchers before the UI tests.", dll);
                    var assembly = System.Reflection.Assembly.LoadFrom(dll);
                    var type = assembly.GetTypes().Single(t => t.Name == "Form1");
                    using var form = (Form)Activator.CreateInstance(type);
                    AssertText(form, "radioButton1", "Use one profile for all versions");
                    AssertText(form, "radioButton2", "Use a separate profile for each version");
                    AssertText(form, "radioButton3", "Use the standard Edge profile");
                    AssertText(form, "button1", "OK");
                    Check(form.Text == name, name + " form, resources and English choices: " + culture);
                    launchers++;
                }
                Check(launchers == 9, "All nine launchers checked: " + culture);
            }
        }
        catch (Exception ex) { failure = ex; }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();
    if (failure != null) throw failure;
    return;
}

if (args.Length == 3 && args[0] == "--install-smoke")
{
    string output = Path.GetFullPath(args[1]);
    Directory.CreateDirectory(output);
    string version = await EdgeUpdateClient.GetVersionAsync("Stable", "X64");
    var package = await EdgeUpdateClient.GetPackageAsync("Stable", "X64", version, default);
    string installer = Path.Combine(output, package.FileId);
    await EdgeUpdateClient.DownloadAsync(package.Url, installer, Convert.FromBase64String(package.Hashes["Sha256"]), null, default);
    Console.WriteLine("PASS: Full installer downloaded over HTTPS and SHA-256 verified.");
    await EdgeInstaller.ExtractAsync(Path.GetFullPath(args[2]), installer, output, default);
    await EdgeInstaller.ExtractAsync(Path.GetFullPath(args[2]), Path.Combine(output, "MSEDGE.7z"), output, default);
    string destination = Path.Combine(output, "installed");
    EdgeInstaller.Install(Path.Combine(output, "Chrome-bin"), destination, version, "Stable", "x64");
    Check(File.ReadAllText(Path.Combine(destination, "updates", "Version.log")) == version + "|Stable|x64", "Full installation and version log");
    return;
}

if (args.Length == 2 && args[0] == "--repair")
{
    SandboxAccess.Ensure(args[1]);
    Console.WriteLine("Sandbox read/execute access repaired for browser binaries.");
    return;
}
if (args.Length == 1 && args[0] == "--online")
{
    foreach (string channel in new[] { "Stable", "Beta", "Dev", "Canary" })
        foreach (string arch in new[] { "X86", "X64" })
        {
            string version = await EdgeUpdateClient.GetVersionAsync(channel, arch);
            var package = await EdgeUpdateClient.GetPackageAsync(channel, arch, version, default);
            Check(package.SizeInBytes > 0 && Convert.FromBase64String(package.Hashes["Sha256"]).Length == 32, "Online package metadata");
            Console.WriteLine($"PASS {channel} {arch}: {version}");
        }
    Check((await EdgeUpdateClient.GetPoliciesAsync()).Count > 0, "Policy metadata");
    return;
}

string root = Path.Combine(Path.GetTempPath(), "PortableEdge-tests-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);
try
{
    string[] forwarded = { "https://example.test/?a=1&b=two words", @"C:\some folder\O'Brien.pdf", "--custom=a=b", "a\"b", @"C:\ends with slash\" };
    var start = EdgeLaunch.CreateStartInfo(root, "Edge", "--user-data-dir=\"profile\"", forwarded);
    Check(start.WorkingDirectory == Path.Combine(root, "Edge") && !start.UseShellExecute, "Explicit browser directory");
    Check(start.ArgumentList[0] == "--user-data-dir=" + Path.Combine(root, "profile"), "Portable profile path");
    Check(start.ArgumentList.Skip(1).SequenceEqual(forwarded), "Arguments preserved without reparsing");
    string absolute = Path.Combine(root, "O'Brien profile");
    Check(EdgeLaunch.CreateStartInfo(root, "Edge", "--user-data-dir=\"" + absolute + "\"", []).ArgumentList[0] == "--user-data-dir=" + absolute, "Absolute profile path");
    Check(EdgeLaunch.CreateStartInfo(root, "Edge", "", forwarded).ArgumentList.SequenceEqual(forwarded), "Default profile");
    Expect<InvalidDataException>(() => EdgeLaunch.CreateStartInfo(root, "Edge", "--user-data-dir=\"\"", []), "Empty profile rejected");

    var packages = new[] {
        new EdgePackage("MicrosoftEdge_X64_153.0.1.2_152.0.1.2.exe", "https://example.test/delta", 1, new()),
        new EdgePackage("MicrosoftEdge_X64_153.0.1.2.exe", "https://example.test/full", 2, new())
    };
    Check(EdgeUpdateClient.SelectFullPackage(packages, "x64", "153.0.1.2").SizeInBytes == 2, "Full package selected instead of delta");
    Expect<InvalidDataException>(() => EdgeUpdateClient.SelectFullPackage(packages, "x86", "153.0.1.2"), "Wrong architecture rejected");
    var secure = EdgeUpdateClient.GetSecureDownloadUri("http://msedge.f.tlu.dl.delivery.mp.microsoft.com/file?signature=a%2Fb");
    Check(secure.Scheme == "https" && secure.Host == "msedge.sf.tlu.dl.delivery.mp.microsoft.com" && secure.Query == "?signature=a%2Fb", "Microsoft secure CDN host and signature");
    Check(NormalizeAcl("D:(A;ID;FA;;;SY)") == NormalizeAcl("D:AI(A;ID;FA;;;SY)"), "ACL comparison permits Windows auto-inheritance bookkeeping");
    Check(NormalizeAcl("D:(A;ID;FA;;;SY)") != NormalizeAcl("D:P(A;ID;FA;;;SY)"), "ACL comparison preserves inheritance protection");
    Check(NormalizeAcl("D:(A;ID;FA;;;SY)") != NormalizeAcl("D:(A;ID;FR;;;SY)"), "ACL comparison detects changed access rights");

    string browser = Path.Combine(root, "Edge");
    string version = Path.Combine(browser, "153.0.1.2");
    Directory.CreateDirectory(version);
    Directory.CreateDirectory(Path.Combine(browser, "profile"));
    File.WriteAllText(Path.Combine(browser, "profile", "secret.txt"), "private");
    File.WriteAllText(Path.Combine(browser, "Profile.txt"), "--user-data-dir=\"profile\"");
    File.WriteAllText(Path.Combine(browser, "msedge.exe"), "test executable");
    File.WriteAllText(Path.Combine(version, "msedge.dll"), "test DLL");
    var profileBefore = Acl(Path.Combine(browser, "profile"));
    var settingsBefore = Acl(Path.Combine(browser, "Profile.txt"));
    SandboxAccess.Ensure(browser);
    var profileAfter = Acl(Path.Combine(browser, "profile"));
    var settingsAfter = Acl(Path.Combine(browser, "Profile.txt"));
    if (profileAfter != profileBefore || settingsAfter != settingsBefore)
    {
        Console.WriteLine("Test profile ACL before: " + profileBefore);
        Console.WriteLine("Test profile ACL after:  " + profileAfter);
        Console.WriteLine("Test settings ACL before: " + settingsBefore);
        Console.WriteLine("Test settings ACL after:  " + settingsAfter);
    }
    Check(profileAfter == profileBefore && settingsAfter == settingsBefore, "Profile permissions untouched");
    foreach (string sid in new[] { "S-1-15-2-1", "S-1-15-2-2" })
        Check(new FileInfo(Path.Combine(version, "msedge.dll")).GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier))
            .Cast<FileSystemAccessRule>().Any(rule => rule.IdentityReference.Value == sid && rule.AccessControlType == AccessControlType.Allow &&
                (rule.FileSystemRights & FileSystemRights.ReadAndExecute) == FileSystemRights.ReadAndExecute), "Sandbox binary access: " + sid);
    string first = Acl(Path.Combine(version, "msedge.dll"));
    SandboxAccess.Ensure(browser);
    Check(first == Acl(Path.Combine(version, "msedge.dll")), "Repair is idempotent");
    Expect<InvalidDataException>(() => EdgeInstaller.Install(Path.Combine(root, "missing"), browser, "153.0.1.2", "Stable", "x64"), "Incomplete install rejected");
    Check(File.ReadAllText(Path.Combine(browser, "msedge.exe")) == "test executable", "Failed validation preserves installed browser");
    string fixtureExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
    string fixtureVersion = FileVersionInfo.GetVersionInfo(fixtureExe).FileVersion;
    string extracted = Path.Combine(root, "extracted");
    string extractedVersion = Path.Combine(extracted, fixtureVersion);
    Directory.CreateDirectory(extractedVersion);
    File.Copy(fixtureExe, Path.Combine(extractedVersion, "msedge.exe"));
    File.WriteAllText(Path.Combine(extractedVersion, "msedge.dll"), "replacement DLL");
    File.WriteAllText(Path.Combine(extractedVersion, "icudtl.dat"), "replacement ICU");
    string existingVersion = Path.Combine(browser, fixtureVersion);
    Directory.CreateDirectory(existingVersion);
    File.WriteAllText(Path.Combine(existingVersion, "old-marker"), "old installation");
    using (var locked = new FileStream(Path.Combine(browser, "msedge.exe"), FileMode.Open, FileAccess.Read, FileShare.Read))
    {
        bool failed = false;
        try { EdgeInstaller.Install(extracted, browser, fixtureVersion, "Stable", "x64"); }
        catch (IOException) { failed = true; }
        catch (UnauthorizedAccessException) { failed = true; }
        Check(failed, "Locked executable rolls back installation");
    }
    Check(File.Exists(Path.Combine(existingVersion, "old-marker")) && File.Exists(Path.Combine(extractedVersion, "msedge.dll")), "Previous version and staged files restored");
    EdgeInstaller.Install(extracted, browser, fixtureVersion, "Stable", "x64");
    Check(File.ReadAllText(Path.Combine(browser, "updates", "Version.log")) == fixtureVersion + "|Stable|x64", "Successful install commits version log");
    Check(File.ReadAllText(Path.Combine(browser, "profile", "secret.txt")) == "private", "Successful install preserves profile data");
    Console.WriteLine("All regression checks passed.");
}
finally { Directory.Delete(root, true); }

static void Check(bool condition, string description)
{
    if (!condition) throw new Exception("FAIL: " + description);
    Console.WriteLine("PASS: " + description);
}
static void Expect<T>(Action action, string description) where T : Exception
{
    try { action(); } catch (T) { Console.WriteLine("PASS: " + description); return; }
    throw new Exception("FAIL: " + description);
}
static string Acl(string path) => NormalizeAcl((Directory.Exists(path) ? (FileSystemSecurity)new DirectoryInfo(path).GetAccessControl() : new FileInfo(path).GetAccessControl()).GetSecurityDescriptorSddlForm(AccessControlSections.Access));
static string NormalizeAcl(string sddl)
{
    var descriptor = new RawSecurityDescriptor(sddl);
    // Writing a parent's DACL can make Windows set the child's AI bookkeeping flag
    // without changing any ACE. Keep every rule, its order, and protection flags.
    descriptor.SetFlags(descriptor.ControlFlags & ~ControlFlags.DiscretionaryAclAutoInherited);
    return descriptor.GetSddlForm(AccessControlSections.Access);
}
static void AssertText(Control parent, string name, string expected)
{
    var control = parent.Controls.Find(name, true).Single();
    if (control.Text != expected) throw new Exception($"Unexpected UI text for {name}: {control.Text}");
}
static IEnumerable<string> MenuTexts(ToolStripItemCollection items)
{
    foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
    {
        yield return item.Text;
        foreach (string child in MenuTexts(item.DropDownItems)) yield return child;
    }
}
