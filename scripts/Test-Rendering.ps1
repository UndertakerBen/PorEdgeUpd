param(
    [Parameter(Mandatory)][string]$EdgePath,
    [Parameter(Mandatory)][string]$OutputDirectory
)
$ErrorActionPreference = 'Stop'
$output = [IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Path $output -Force | Out-Null
$profile = Join-Path $output ('profile-' + [guid]::NewGuid().ToString('N'))
$html = Join-Path $output 'render.html'
Set-Content -LiteralPath $html -Value '<!doctype html><title>Portable Edge render test</title><p id="result">waiting</p><script>document.getElementById("result").textContent="render-ok"</script>'
$info = [Diagnostics.ProcessStartInfo]::new([IO.Path]::GetFullPath($EdgePath))
$info.UseShellExecute = $false
$info.CreateNoWindow = $true
$info.RedirectStandardOutput = $true
$info.RedirectStandardError = $true
foreach ($arg in @('--headless=new', '--edge-skip-compat-layer-relaunch', '--no-first-run', '--no-default-browser-check', "--user-data-dir=$profile", '--enable-logging=stderr', '--dump-dom', ([Uri]$html).AbsoluteUri)) {
    $info.ArgumentList.Add($arg)
}
$process = [Diagnostics.Process]::Start($info)
$stdout = $process.StandardOutput.ReadToEndAsync()
$stderr = $process.StandardError.ReadToEndAsync()
$finished = $process.WaitForExit(20000)
if (!$finished) { $process.Kill($true); $process.WaitForExit() }
$text = $stdout.GetAwaiter().GetResult()
Set-Content -LiteralPath (Join-Path $output 'stdout.txt') -Value $text
Set-Content -LiteralPath (Join-Path $output 'stderr.txt') -Value $stderr.GetAwaiter().GetResult()
if (!$finished -or $process.ExitCode -ne 0 -or $text -notmatch '<p id="result">render-ok</p>') {
    throw "Rendering failed. See $output."
}
Write-Output 'PASS: sandboxed Edge rendered HTML and executed JavaScript.'
