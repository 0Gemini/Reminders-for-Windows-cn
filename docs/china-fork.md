# Gemini Reminders CN

Personal China-region fork for 0Gemini, based on Paul Savvas's Reminders for Windows (upstream snapshot 5450bcd). The original MIT license and copyright remain in LICENSE.

This first version is China-region only. It uses https://www.icloud.com.cn/reminders/ and https://setup.icloud.com.cn/setup/ws/1. Apple ID authentication still uses idmsa.apple.com. Browser navigation, account-response detection, session-cookie validation, OAuth origin/redirect, and CloudKit service URL validation use the China region. Service URLs still come from Apple's account response; arbitrary URLs and lookalike domains are rejected.

App name: Gemini Reminders CN. Package identity: Gemini.RemindersCN, publisher CN=0Gemini. Cache/logs use %LOCALAPPDATA%/GeminiRemindersCNSync; browser profile/preferences use %LOCALAPPDATA%/GeminiRemindersCN. Credential Manager services use com.0gemini.reminders-cn. Existing Store app data is not migrated. Update checks target 0Gemini/Reminders-for-Windows-cn.

## Build and run

Open the Actions tab in this fork and run Build Gemini Reminders CN, or push a main/codex branch. Download the Gemini-Reminders-CN-x64 artifact, extract it completely, and run Reminders.exe. The workflow builds a portable app only; it does not publish to Microsoft Store or create a release. Original workflows are retained under .github/upstream-workflows for reference and do not run.

For a local Windows build with .NET 8 SDK:

    ./scripts/build-windows.ps1 -Architecture x64 -LockedRestore

The output is dist-windows/Reminders.exe. A local MSIX would require a certificate matching CN=0Gemini; no certificate or signing key is included. Upstream Inno/Store packaging scripts are not adapted for this fork and must not be used to install over the original app.

## Validation

Core and scheduling tests, including China navigation, cookie domain rejection, and request-header checks: 1239 checks passed locally. These tests use synthetic sessions and do not prove live Apple interoperability. Live China-account sign-in, session restoration, list reading, and two-way edits must still be verified by the account owner. Prefer the embedded Apple browser sign-in for the first test.

First verify login and list reading. Then use a disposable test reminder to check creation, editing and completion in both directions with an Apple device. Close and reopen the app to check session restoration. Do not report the fork as fully working until these checks succeed.