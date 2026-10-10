using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Edge_Updater
{
    internal sealed record EdgePackage(string FileId, string Url, long SizeInBytes, Dictionary<string, string> Hashes);
    internal sealed record PolicyPackage(string Version, string Extension, string Url, string Sha256);

    internal static class EdgeUpdateClient
    {
        private static readonly HttpClient Client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

        public static async Task<string> GetVersionAsync(string channel, string architecture)
        {
            string url = $"https://msedge.api.cdp.microsoft.com/api/v1.1/contents/Browser/namespaces/Default/names/msedge-{channel.ToLowerInvariant()}-win-{architecture.ToLowerInvariant()}/versions/latest?action=select";
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using var response = await Client.PostAsJsonAsync(url, new { targetingAttributes = new { Updater = "MicrosoftEdgeUpdate" } }, timeout.Token);
            response.EnsureSuccessStatusCode();
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(timeout.Token));
            string version = json.RootElement.GetProperty("ContentId").GetProperty("Version").GetString();
            if (!Version.TryParse(version, out _))
                throw new InvalidDataException("Microsoft returned an invalid Edge version.");
            return version;
        }

        public static EdgePackage SelectFullPackage(IEnumerable<EdgePackage> packages, string architecture, string version)
        {
            string name = $"MicrosoftEdge_{architecture.ToUpperInvariant()}_{version}.exe";
            return packages.SingleOrDefault(package => package.FileId.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidDataException("The full Edge installer is missing from Microsoft's response.");
        }

        public static async Task<EdgePackage> GetPackageAsync(string channel, string architecture, string version, CancellationToken cancellationToken)
        {
            string url = $"https://msedge.api.cdp.microsoft.com/api/v1.1/internal/contents/Browser/namespaces/Default/names/msedge-{channel}-win-{architecture}/versions/{version}/files?action=GenerateDownloadInfo&foregroundPriority=true";
            using var response = await Client.PostAsJsonAsync(url, new { }, cancellationToken);
            response.EnsureSuccessStatusCode();
            var packages = await response.Content.ReadFromJsonAsync<EdgePackage[]>(cancellationToken);
            return SelectFullPackage(packages, architecture, version);
        }

        public static async Task<List<PolicyPackage>> GetPoliciesAsync()
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using var response = await Client.GetAsync("https://edgeupdates.microsoft.com/api/products?view=enterprise", timeout.Token);
            response.EnsureSuccessStatusCode();
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(timeout.Token));
            var result = new List<PolicyPackage>();
            foreach (var product in json.RootElement.EnumerateArray())
            {
                if (product.GetProperty("Product").GetString() != "Policy") continue;
                foreach (var release in product.GetProperty("Releases").EnumerateArray())
                    foreach (var artifact in release.GetProperty("Artifacts").EnumerateArray())
                    {
                        string extension = artifact.GetProperty("ArtifactName").GetString();
                        if (extension != "zip" && extension != "cab") continue;
                        if (artifact.GetProperty("HashAlgorithm").GetString() != "SHA256") continue;
                        result.Add(new PolicyPackage(release.GetProperty("ProductVersion").GetString(), extension,
                            artifact.GetProperty("Location").GetString(), artifact.GetProperty("Hash").GetString()));
                    }
            }
            return result;
        }

        public static Uri GetSecureDownloadUri(string url)
        {
            // CDP's f host is HTTP-only. Its sf counterpart serves the same file over TLS.
            var uri = new UriBuilder(url);
            if (uri.Host == "msedge.f.tlu.dl.delivery.mp.microsoft.com")
                uri.Host = "msedge.sf.tlu.dl.delivery.mp.microsoft.com";
            else if (uri.Host == "msedge.f.dl.delivery.mp.microsoft.com")
                uri.Host = "msedge.sf.dl.delivery.mp.microsoft.com";
            if (uri.Scheme == "http") { uri.Scheme = "https"; uri.Port = -1; }
            if (uri.Scheme != "https") throw new InvalidDataException("Download URL must use HTTPS.");
            return uri.Uri;
        }

        public static async Task DownloadAsync(string url, string destination, byte[] expectedHash,
            IProgress<(long Received, long? Total)> progress, CancellationToken cancellationToken)
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromMinutes(10));
            cancellationToken = timeout.Token;
            if (expectedHash.Length != 32) throw new InvalidDataException("Missing SHA-256 download checksum.");
            using var response = await Client.GetAsync(GetSecureDownloadUri(url), HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            await using var input = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using var output = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);
            using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            byte[] buffer = new byte[81920];
            long total = 0;
            int count;
            while ((count = await input.ReadAsync(buffer, cancellationToken)) != 0)
            {
                await output.WriteAsync(buffer.AsMemory(0, count), cancellationToken);
                hash.AppendData(buffer, 0, count);
                total += count;
                progress?.Report((total, response.Content.Headers.ContentLength));
            }
            if (!CryptographicOperations.FixedTimeEquals(hash.GetHashAndReset(), expectedHash))
                throw new InvalidDataException("The downloaded file failed SHA-256 verification. Nothing was installed.");
        }
    }
}
