#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/* This script implements the [Volume Rendering -> Create Single-Channel Asset from Image Directory]
feature located in the editor menu. This was made before the switch to IMS files, so it takes in a 
sequence of PNG or JPG files and creates a .asset file from them. This script in particluar is made for 
the creation of single channel assets. For multichannel assets and video assets, see 
MultiChannelAssetBuilder.cs, VideoAssetBuilder.cs, or ImarisAssetBuilder.cs */

public class SingleChannelAssetBuilder : EditorWindow
{
    // As of now, this script can only parse PNG and JPG
    string imageExt = "png";
    // Desired output path
    string assetPath = "Assets/VolumeRendering/TextureStacks/Stack.asset";

    // Add single channel asset builder feature to the editor menu
    [MenuItem("Volume Rendering/Create Single-Channel Asset from Image Directory")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SingleChannelAssetBuilder window = (SingleChannelAssetBuilder)EditorWindow.GetWindow(typeof(SingleChannelAssetBuilder));
        window.Show();
    }

    // Customize inputs and interface of the feature
    void OnGUI()
    {
        // Ajustable file extension (PNG or JPG)
        GUILayout.Label("Image File Extension", EditorStyles.boldLabel);
        imageExt = EditorGUILayout.TextField(imageExt);

        // Adjustable asset output path
        GUILayout.Label("Asset Output Path", EditorStyles.boldLabel);
        assetPath = EditorGUILayout.TextField(assetPath);

        // Once the user presses the "Build" button, they will choose the folder of PNG or JPG images
        // that they wish to convert into a .asset file
        if (GUILayout.Button("Build"))
        {
            BuildStack();
        }
    }

    // Convert a folder of PNG or JPG images into a Texture3D object, and save the Texture3D object
    // as a .asset file
    void BuildStack()
    {
        // The OpenFolderPanel function opens a Windows prompt that allows the user to browse
        // their computer and choose an input folder
        string imageDirectory = EditorUtility.OpenFolderPanel("Choose Image Directory", "", "");
        // Store all of the paths of the files stored in the selected directory
        string[] files = Directory.GetFiles(imageDirectory, "*." + imageExt);

        // image dimensions (correct dimensions are set later)
        int width = -1;
        int height = -1;

        // Creates an array called "layers" that has the same length as the number of PNGs or JPGS in "files"
        Texture2D[] layers = new Texture2D[files.Length];

        // Fill "layers" with textures created from the folder that has all of the PNGs or JPGs
        Texture2D tex = null;
        byte[] imageData;
        for (int i = 0; i < files.Length; i++)
        {
            // File path of the i'th image in the input directory
            string file = files[i];

            // If the file exists, read all of its data and create a Texture2D from it
            if (File.Exists(file))
            {
                // Read all of the image's pixel data into the "imageData" byte (8-bit unsigned integer) array
                imageData = File.ReadAllBytes(file);
                // Initialize a new texture using the LoadImage function, which takes in the array of pixel data
                tex = new Texture2D(1, 1);
                tex.LoadImage(imageData);
            }
            else
            {
                // Image does not exist
                Debug.Log("Error loading " + file + ": file doesn't exist");
            }

            // Initialize image width and height (if not done already)
            if (width == -1 || height == -1)
            {
                // After filling the Texture2D will the pixel data, we can now access Texture2D's member variables
                // such as width and height. For a more details on texture objects see:
                // https://docs.unity3d.com/ScriptReference/Texture2D.html
                width = tex.width;
                height = tex.height;
            }

            // Make sure that all of the images in the image stack have the same dimensions
            if (width != tex.width || height != tex.height)
            {
                Debug.Log("Error loading " + file + ": images must have the same dimensions");
                return;
            }

            // Added texture to "layers" array
            layers[i] = tex;
            Debug.Log(file + " loaded successfully");
        }

        // No images were found or able to be parsed
        if (layers.Length == 0)
        {
            Debug.Log("Error: no images found");
            return;
        }

        Debug.Log("Width: " + width);
        Debug.Log("Height: " + height);
        Debug.Log("Layers: " + layers.Length);

        // Create a Texture3D object using the now populated "layers" array
        // "layers" now carries all of the slices of the texture stack
        Texture3D stack = new Texture3D(width, height, layers.Length, TextureFormat.ARGB32, false);
        stack.wrapMode = TextureWrapMode.Clamp;
        stack.filterMode = FilterMode.Bilinear;
        stack.anisoLevel = 0;

        // Populate the "pixels" array with all of the pixel colors from "layers"
        int totalPixels = width * height * layers.Length;
        Color32[] pixels = new Color32[totalPixels];
        int count = 0;
        foreach (Texture2D layer in layers)
        {
            Color32[] texPixels = layer.GetPixels32();
            foreach (Color32 pixel in texPixels)
            {
                pixels[count] = pixel;
                count++;
            }
        }

        // Set the Texture3D's pixel colors using the now populated "pixels" array
        stack.SetPixels32(pixels);
        stack.Apply();

        // Save the Texture3D object in the desired ouput location
        // Saving the asset converts it to a .asset file
        AssetDatabase.CreateAsset(stack, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif 