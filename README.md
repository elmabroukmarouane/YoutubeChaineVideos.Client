# Deploying a Blazor WebAssembly App and Publishing an Android App

This guide outlines the steps to deploy a Blazor WebAssembly app to IIS and publish an Android app.

---

## Deploying a Blazor WebAssembly App to IIS

### 1. Prepare Your Blazor WebAssembly App
1. **Build the app**:
   - Open the Blazor WebAssembly project in Visual Studio.
   - Set the build configuration to `Release` (use the dropdown in the toolbar).
2. **Publish the app**:
   - Right-click the project in **Solution Explorer** and select **Publish**.
   - Choose **Folder** as the target.
   - Set the folder path where the files will be published.
   - Click **Finish** to generate the necessary files.

---

### 2. Install and Configure IIS
1. **Install IIS**:
   - Go to **Control Panel > Programs > Turn Windows Features On or Off**.
   - Enable:
     - Internet Information Services (IIS)
     - Web Management Tools
     - World Wide Web Services
       - Application Development Features: Enable `.NET Extensibility 4.7+` and `ASP.NET 4.7+`.

2. **Open IIS Manager**:
   - Press `Windows + R`, type `inetmgr`, and press Enter.

---

### 3. Set Up the IIS Website
1. **Create a new website**:
   - In IIS Manager, right-click **Sites** and select **Add Website**.
   - Enter:
     - **Site Name**: e.g., `YoutubeChaineVideos.Client.Web`
     - **Physical Path**: Path to the published Blazor files.
     - **Set HostName**: e.g `youtube-channels-management.web.local`
     - **Setting hosts**: Modify the `hosts` file located at `C:\Windows\System32\drivers\etc\hosts` to include the following entries: `127.0.0.1 youtube-channels-management.web.local`. This config is optional and you can adapt the hostname as preffered
     - **Port**: e.g., 5000 (or leave the default, 80 if you're using hostname).
   - Click **OK**.

2. **Configure MIME Types**:
   - Select the site in IIS Manager.
   - Double-click **MIME Types** in the Features View.
   - Ensure the following MIME types are added:
     - `.dll` -> `application/octet-stream`
     - `.wasm` -> `application/wasm`
     - `.json` -> `application/json`
     - `.woff` -> `font/woff`
     - `.woff2` -> `font/woff2`
     - `.mp4` -> `video/mp4`

---

### 4. Enable Static File Hosting
1. Double-click **Request Filtering** in the Features View.
2. Select **Edit Feature Settings** from the right-hand pane.
3. Ensure **Allow unlisted file extensions** is checked.

---

### 5. Configure the Application Pool
1. Select your site in IIS Manager.
2. Click **Advanced Settings** in the Actions pane.
3. Set the Application Pool properties:
   - **Managed Pipeline Mode**: `Integrated`
   - **.NET CLR Version**: `No Managed Code`

---

### 6. Run and Test the Application
1. **Browse the site**:
   - In IIS Manager, click **Browse *:port** in the Actions pane.
   - Replace `*:port` with the port number assigned during setup (e.g., `http://localhost:5000`).
2. **Verify**:
   - Ensure the Blazor app loads correctly in your browser.

---

## Publishing an Android App

### 1. Prepare Your Android App
1. Open your Android project in Android Studio.
2. Set the build configuration to `Release`:
   - Go to **Build > Select Build Variant** and choose `release` for your app module.
   - Add `android:usesCleartextTraffic="true"` to `<application android:usesCleartextTraffic="true" ...></application>` in `AndroidManifest.xml`, if you are using `http`. Not required when using `https`
3. Sign the app:
   - Go to **Build > Generate Signed Bundle / APK**.
   - Select **APK** and click **Next**.
   - Choose or create a **key store** file and enter the required credentials.
   - Set the **key alias** and password, then click **Next**.
   - Select **Release** build and click **Finish**.

---

### 2. Generate and Export the APK
1. Wait for Android Studio to generate the signed APK file.
2. The APK file will be saved in the specified output directory, typically `app/release`.

---

### 3. Publish the APK
1. **Google Play Store**:
   - Log in to the [Google Play Console](https://play.google.com/console).
   - Create a new app or select an existing one.
   - Upload the signed APK under **Production > Releases**.
   - Complete the app details and submit for review.
2. **Direct Distribution**:
   - Share the APK file directly via a website, email, or other methods.
