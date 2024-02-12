﻿using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEngine;
using UnityEditor.iOS.Xcode;
using System.IO;
#endif

namespace Gley.MobileAds.Internal
{
    public class PostBuildStep
    {
        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                AddPListValues(pathToXcode);
            }
        }

        // Implement a function to read and write values to the plist file:
        static void AddPListValues(string pathToXcode)
        {
#if UNITY_IOS
#if GLEY_ATT
        // Retrieve the plist file from the Xcode project directory:
        string plistPath = pathToXcode + "/Info.plist";
        PlistDocument plistObj = new PlistDocument();


        // Read the values from the plist file:
        plistObj.ReadFromString(File.ReadAllText(plistPath));

        // Set values from the root object:
        PlistElementDict plistRoot = plistObj.root;

        // Set the description key-value in the plist:
        plistRoot.SetString("NSUserTrackingUsageDescription", ResourcesContainer.Load<MobileAdsData>(Constants.DATA_NAME_RUNTIME).nativePopupText);

        // Save changes to the plist:
        File.WriteAllText(plistPath, plistObj.WriteToString());
#endif
#endif
        }
    }
}