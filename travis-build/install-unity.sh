#! /bin/sh

# See https://unity3d.com/get-unity/download/archive
# to get download URLs
UNITY_DOWNLOAD_CACHE="$(pwd)/unity_download_cache"
UNITY_OSX_PACKAGE_URL="https://download.unity3d.com/download_unity/9e14d22a41bb/MacEditorInstaller/Unity.pkg?_ga=2.99758302.1799409747.1555029489-42546876.1546952479"
UNITY_ANDROID_TARGET_PACKAGE_URL="https://download.unity3d.com/download_unity/9e14d22a41bb/MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-2018.3.7f1.pkg?_ga=2.99758302.1799409747.1555029489-42546876.1546952479"

# Downloads a file if it does not exist
download() {

	URL=$1
	FILE=`basename "$URL"`
	
	# Downloads a package if it does not already exist in cache
	if [ ! -e $UNITY_DOWNLOAD_CACHE/$2 ] ; then
		echo "$FILE does not exist. Downloading from $URL: "
		mkdir -p "$UNITY_DOWNLOAD_CACHE"
		curl -o $UNITY_DOWNLOAD_CACHE/$2` "$URL"
		ls $UNITY_DOWNLOAD_CACHE
	else
		echo "$FILE Exists. Skipping download."
	fi
}

# Downloads and installs a package from an internet URL
install() {
	PACKAGE_URL=$1
	PACKAGE_NAME = $2
	download $1 $2

	echo "Installing $sPACKAGE_NAME"
	sudo installer -dumplog -package $UNITY_DOWNLOAD_CACHE/`basename "$PACKAGE_URL"` -target /
}



echo "Contents of Unity Download Cache:"
ls $UNITY_DOWNLOAD_CACHE

echo "Installing Unity..."
install $UNITY_OSX_PACKAGE_URL "unity_osx"
install $UNITY_ANDROID_TARGET_PACKAGE_URL "unity_android"
