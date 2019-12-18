using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Transform), typeof(MeshFilter), typeof(MeshRenderer))]
public class VolumeRenderer : MonoBehaviour
{
    // Asset formats
    public enum Option
    {
        SINGLECHANNEL = 0,
        MULTICHANNEL,
        VIDEO,
        MASK
    }

    // double buffering for video
    public enum BufferType
    {
        BACKBUFFER = 0,
        FRONTBUFFER
    }


    [Range(0, 10)] public float xscale = 1;
    [Range(0, 10)] public float yscale = 1;
    [Range(0, 10)] public float zscale = 1;


    public Option AssetFormat = Option.SINGLECHANNEL;

    public string AssetPath;

    // volume dimension parameters
    [Range(-0.5f, 0.5f)]
    public float pwidth = 0.5f;
    [Range(-0.5f, 0.5f)]
    public float pheight = 0.5f;
    [Range(-0.5f, 0.5f)]
    public float pdepth = 0.5f;
    //Kyle crop test
    [Range(0.5f, -0.5f)]
    public float nwidth = -0.5f;
    [Range(0.5f, -0.5f)]
    public float nheight = -0.5f;
    [Range(0.5f, -0.5f)]
    public float ndepth = -0.5f;

    [Range(0.01f, 1f)]
    public float width_scale = 1;
    [Range(0.01f, 1f)]
    public float height_scale = 1;
    [Range(0.01f, 1f)]
    public float depth_scale = 1;

    [Range(10, 1000)]
    public int Steps = 50;

    [Range(1, 14)]
    public int NumChannels = 1;

    [Range(0f, 2f)]
    public float PlaybackSpeed = 1.0f;

    // threshold value for empty space check
    [Range(0f, 1f)]
    public float minAlpha = 0.004f;

    [Range(0f, 1f)]
    public float GlobalAttenuation = 0.001f;

    [Range(0f, 1f)]
    public float HiddenAttenuation = 0.001f;

    [Range(0f, 2f)]
    public float GlobalBrightness = 0.000f;

    [Range(0f, 1f)]
    public float FilterThreshold = 0.000f;

    public Color Channel0Color = Color.cyan;
    public Color Channel1Color = Color.red;
    public Color Channel2Color = Color.green;
    public Color Channel3Color = Color.magenta;
    public Color Channel4Color = Color.yellow;
    public Color Channel5Color = Color.white;
    public Color Channel6Color = Color.blue;

    [Range(0f, 1f)]
    public float R = 1f;
    [Range(0f, 1f)]
    public float G = 1f;
    [Range(0f, 1f)]
    public float B = 1f;

    [Range(0f, 1f)]
    public float Channel0Amount = 1f;
    [Range(0f, 1f)]
    public float Channel1Amount = 1f;
    [Range(0f, 1f)]
    public float Channel2Amount = 1f;
    [Range(0f, 1f)]
    public float Channel3Amount = 1f;
    [Range(0f, 1f)]
    public float Channel4Amount = 1f;
    [Range(0f, 1f)]
    public float Channel5Amount = 1f;
    [Range(0f, 1f)]
    public float Channel6Amount = 1f;

    [Range(0f, 1f)]
    public float Channel0GlobalAttenuation = 0.001f;
    [Range(0f, 1f)]
    public float Channel1GlobalAttenuation = 0.001f;
    [Range(0f, 1f)]
    public float Channel2GlobalAttenuation = 0.001f;
    [Range(0f, 1f)]
    public float Channel3GlobalAttenuation = 0.001f;
    [Range(0f, 1f)]
    public float Channel4GlobalAttenuation = 0.001f;
    [Range(0f, 1f)]
    public float Channel5GlobalAttenuation = 0.001f;
    [Range(0f, 1f)]
    public float Channel6GlobalAttenuation = 0.001f;

    [Range(0f, 2f)]
    public float Channel0GlobalBrightness = 0.000f;
    [Range(0f, 2f)]
    public float Channel1GlobalBrightness = 0.000f;
    [Range(0f, 2f)]
    public float Channel2GlobalBrightness = 0.000f;
    [Range(0f, 2f)]
    public float Channel3GlobalBrightness = 0.000f;
    [Range(0f, 2f)]
    public float Channel4GlobalBrightness = 0.000f;
    [Range(0f, 2f)]
    public float Channel5GlobalBrightness = 0.000f;
    [Range(0f, 2f)]
    public float Channel6GlobalBrightness = 0.000f;

    [Range(0f, 1f)]
    public float Channel0FilterThreshold = 0.000f;
    [Range(0f, 1f)]
    public float Channel1FilterThreshold = 0.000f;
    [Range(0f, 1f)]
    public float Channel2FilterThreshold = 0.000f;
    [Range(0f, 1f)]
    public float Channel3FilterThreshold = 0.000f;
    [Range(0f, 1f)]
    public float Channel4FilterThreshold = 0.000f;
    [Range(0f, 1f)]
    public float Channel5FilterThreshold = 0.000f;
    [Range(0f, 1f)]
    public float Channel6FilterThreshold = 0.000f;

    [Range(0f, 1f)]
    public float GamaLowerBound = 0.000f;

    [Range(0f, 1f)]
    public float GamaUpperBound = 1.000f;

    [Range(0f, 2f)]
    public float GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float AlphaTest = 1.0f;

    [Range(0f, 1f)]
    public float Channel00GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel00GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel00GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float Channel01GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel01GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel01GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float Channel02GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel02GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel02GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float Channel03GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel03GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel03GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float Channel04GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel04GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel04GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float Channel05GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel05GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel05GamaCurveValue = 1.0f;

    [Range(0f, 1f)]
    public float Channel06GamaLowerBound = 0.0f;
    [Range(0f, 1f)]
    public float Channel06GamaUpperBound = 1.0f;
    [Range(0f, 2f)]
    public float Channel06GamaCurveValue = 1.0f;

    public float[] Channel_GammaLowerBound = new float[7];
    public float[] Channel_GammaUpperBound = new float[7];
    public float[] Channel_GammaCurveValue = new float[7];

    // video update parameters
    private int Tick = 0;
    private float TimeSinceLastFrame = 0.0f;
    private BufferType DisplayBuffer = BufferType.FRONTBUFFER;


    private Vector3 dimensions;

    // material to apply to the volume
    private Material material;

    private Transform ThisTransform;

    private int ScaleCount;

    //*********************************************************************************************
    //Previous Asset Format before each time clock and Next Asset Format after each time clock.
    private Option PreviousAssetFormat;
    private Option NextAssetFormat;

    //Previous Asset Path before each time clock and Next Asset Path after each time clock.
    private string PreviousAssetPath;
    private string NextAssetPath;
    //*********************************************************************************************

    FPSCalculator ThisFPSCalculator;

    void Start()
    {
        Initialize();

    }

    public void Initialize()
    {


            for (int i = 0; i < 7; i++)
            {
                Channel_GammaLowerBound[i] = 0.0f;
                Channel_GammaUpperBound[i] = 1.0f;
                Channel_GammaCurveValue[i] = 0.5f;
            }


            // for Coroutine test use
            StartCoroutine(LoadTexture());

            string PathRetrieved = PlayerPrefs.GetString("assetPath");
            if (PathRetrieved != "")
            {
                AssetPath = "Assets/" + PathRetrieved;
            }
            Debug.Log("Path retrieved:" + AssetPath);

            if (AssetPath.Contains("Nils"))
            {
                AssetFormat = Option.SINGLECHANNEL;
            }
            else
            {
            AssetFormat = Option.MULTICHANNEL;
            }

            // create cube for raytracing
            GetComponent<MeshFilter>().sharedMesh = buildMesh(pwidth, pheight, pdepth);

            ThisTransform = GetComponent<Transform>();

            ThisFPSCalculator = GetComponent<FPSCalculator>();

            // startup based on asset format
            switch (AssetFormat)
            {
                case Option.SINGLECHANNEL:
                    //Resources.LoadAsync
                    material = (Material)Resources.Load("Materials/SingleChannel", typeof(Material));
                    NextAssetFormat = Option.SINGLECHANNEL;
                    SingleChannelStartup(AssetPath);
                    break;
                case Option.MULTICHANNEL:
                    material = (Material)Resources.Load("Materials/MultiChannel", typeof(Material));
                    NextAssetFormat = Option.MULTICHANNEL;
                    MultiChannelStartup(AssetPath);
                    break;
                case Option.VIDEO:
                    material = (Material)Resources.Load("Materials/Video", typeof(Material));
                    NextAssetFormat = Option.VIDEO;
                    VideoStartup(AssetPath);
                    break;
                case Option.MASK:
                    material = (Material)Resources.Load("Materials/Mask", typeof(Material));
                    NextAssetFormat = Option.MASK;
                    MaskStartup(AssetPath);
                    break;
            }

            //
            NextAssetPath = AssetPath;

            // apply material with shader specified for the selected asset format
            GetComponent<MeshRenderer>().sharedMaterial = material;

            // initialize volume dimensions
            material.SetFloat("_Pwidth", pwidth);
            material.SetFloat("_Pheight", pheight);
            material.SetFloat("_Pdepth", pdepth);
            material.SetFloat("_Nwidth", nwidth);
            material.SetFloat("_Nheight", nheight);
            material.SetFloat("_Ndepth", ndepth);
            transform.localScale = dimensions;

            //
            PreviousAssetFormat = NextAssetFormat;
            PreviousAssetPath = NextAssetPath;

            ScaleCount = 0;

            xscale = transform.localScale.x;
            yscale = transform.localScale.y;
            zscale = transform.localScale.z;

            //GameObject go = GameObject.Find("DesktopCanvas");
            //DesktopSliderManager other = (DesktopSliderManager)go.GetComponent(typeof(DesktopSliderManager));
            //other.SetInitialScaleSliderValue();
    }

    // Nils/Nils1

    IEnumerator LoadTexture()
    {
        ResourceRequest request1 = Resources.LoadAsync("Assets/Nils1/Nils1");

        ResourceRequest request2 = Resources.LoadAsync("Assets/Nils2/Nils2");

        ResourceRequest request3 = Resources.LoadAsync("Assets/Nils3/Nils3");

        ResourceRequest request4 = Resources.LoadAsync("Assets/ZebraFish/Channel0");
        ResourceRequest request5 = Resources.LoadAsync("Assets/ZebraFish/Channel1");
        ResourceRequest request6 = Resources.LoadAsync("Assets/ZebraFish/Channel2");
        ResourceRequest request7 = Resources.LoadAsync("Assets/ZebraFish/Channel3");
        ResourceRequest request8 = Resources.LoadAsync("Assets/ZebraFish/Channel4");
        ResourceRequest request9 = Resources.LoadAsync("Assets/ZebraFish/Channel5");
        ResourceRequest request10 = Resources.LoadAsync("Assets/ZebraFish/Channel6");

        ResourceRequest request11 = Resources.LoadAsync("Assets/ZebraFish/ZebraFishComposite");

        ResourceRequest request12 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel0");
        ResourceRequest request13 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel1");
        ResourceRequest request14 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel2");
        ResourceRequest request15 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel3");
        ResourceRequest request16 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel4");
        ResourceRequest request17 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel5");
        ResourceRequest request18 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 3/Timepoint 0/Channel6");

        ResourceRequest request19 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel0");
        ResourceRequest request20 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel1");
        ResourceRequest request21 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel2");
        ResourceRequest request22 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel3");
        ResourceRequest request23 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel4");
        ResourceRequest request24 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel5");
        ResourceRequest request25 = Resources.LoadAsync("Assets/NIHLung_Reduced/ResolutionLevel 2/Timepoint 0//Channel6");

        yield return request1;
    }

    void LocalScale()
    {
        Matrix4x4 matrix;
        Vector4 v = new Vector4(0, 0, 0, 0);

        matrix = Matrix4x4.identity;

        v.Set(ThisTransform.localScale.x, ThisTransform.localScale.y, ThisTransform.localScale.z, 1);

        matrix.m00 = width_scale;
        matrix.m11 = height_scale;
        matrix.m22 = depth_scale;

        v = matrix * v;

        ThisTransform.localScale = new Vector3(v.x, v.y, v.z);

    }

    // Update shader properties each frame
    void Update()
    {



        Channel00GamaLowerBound = Channel_GammaLowerBound[0];
        Channel00GamaUpperBound = Channel_GammaUpperBound[0];
        Channel00GamaCurveValue = Channel_GammaCurveValue[0];

        Channel01GamaLowerBound = Channel_GammaLowerBound[1];
        Channel01GamaUpperBound = Channel_GammaUpperBound[1];
        Channel01GamaCurveValue = Channel_GammaCurveValue[1];

        Channel02GamaLowerBound = Channel_GammaLowerBound[2];
        Channel02GamaUpperBound = Channel_GammaUpperBound[2];
        Channel02GamaCurveValue = Channel_GammaCurveValue[2];

        Channel03GamaLowerBound = Channel_GammaLowerBound[3];
        Channel03GamaUpperBound = Channel_GammaUpperBound[3];
        Channel03GamaCurveValue = Channel_GammaCurveValue[3];

        Channel04GamaLowerBound = Channel_GammaLowerBound[4];
        Channel04GamaUpperBound = Channel_GammaUpperBound[4];
        Channel04GamaCurveValue = Channel_GammaCurveValue[4];

        Channel05GamaLowerBound = Channel_GammaLowerBound[5];
        Channel05GamaUpperBound = Channel_GammaUpperBound[5];
        Channel05GamaCurveValue = Channel_GammaCurveValue[5];

        Channel06GamaLowerBound = Channel_GammaLowerBound[6];
        Channel06GamaUpperBound = Channel_GammaUpperBound[6];
        Channel06GamaCurveValue = Channel_GammaCurveValue[6];


        // A and B button functionality part
        // ===============================================================================================================================
        float Component_x = this.transform.localScale.x;
        float Component_y = this.transform.localScale.y;
        float Component_z = this.transform.localScale.z;

        float Scale_Scalar = 0.1f;

        //Kyle scale test


        this.transform.localScale = new Vector3(xscale, yscale, zscale);



        if (Input.GetKeyDown("r"))
        {
           
                ScaleCount = ScaleCount + 1;
                this.transform.localScale += new Vector3(Component_x * Scale_Scalar, 0, 0);
            
        }
        if (Input.GetKeyDown("t"))
        {

                ScaleCount = ScaleCount + 1;
                this.transform.localScale += new Vector3(0, Component_y * Scale_Scalar, 0);
            
        }
        if (Input.GetKeyDown("y"))
        {

                ScaleCount = ScaleCount + 1;
                this.transform.localScale += new Vector3(0, 0, Component_z * Scale_Scalar);
            
        }
        if (Input.GetKeyDown("f"))
        {

            ScaleCount = ScaleCount - 1;
            this.transform.localScale -= new Vector3(Component_x * Scale_Scalar, 0, 0);

        }
        if (Input.GetKeyDown("g"))
        {

            ScaleCount = ScaleCount - 1;
            this.transform.localScale -= new Vector3(0, Component_y * Scale_Scalar, 0);

        }
        if (Input.GetKeyDown("h"))
        {

            ScaleCount = ScaleCount - 1;
            this.transform.localScale -= new Vector3(0, 0, Component_z * Scale_Scalar);

        }


        //end Kye scale test

        if (Input.GetAxisRaw("Oculus_GearVR_LThumbstickX") > 0f)
        {
            Debug.Log("press B");
            if (this.transform.localScale.x < 5550f)
            {
                ScaleCount = ScaleCount + 1;
                this.transform.localScale += new Vector3(Component_x * Scale_Scalar, Component_y * Scale_Scalar, Component_z * Scale_Scalar);
            }


        }

        if (Input.GetAxisRaw("Oculus_GearVR_LThumbstickY") > 0f)
        {
            Debug.Log("press A");
            if (this.transform.localScale.x > 0.05f)
            {
                ScaleCount = ScaleCount - 1;
                this.transform.localScale -= new Vector3(Component_x * Scale_Scalar, Component_y * Scale_Scalar, Component_z * Scale_Scalar);
            }

        }

        // ===============================================================================================================================

        //Debug.Log(ThisFPSCalculator.f_Fps);

        //get the current asset format and path in order to check the change
        NextAssetFormat = AssetFormat;
        NextAssetPath = AssetPath;

        if (NextAssetFormat != PreviousAssetFormat || NextAssetPath != PreviousAssetPath)
        {
            Debug.Log("state change");

            switch (NextAssetFormat)
            {
                case Option.SINGLECHANNEL:
                    material = (Material)Resources.Load("Materials/SingleChannel", typeof(Material));
                    NextAssetFormat = Option.SINGLECHANNEL;
                    SingleChannelStartup(NextAssetPath);
                    break;
                case Option.MULTICHANNEL:
                    material = (Material)Resources.Load("Materials/MultiChannel", typeof(Material));
                    NextAssetFormat = Option.MULTICHANNEL;
                    MultiChannelStartup(NextAssetPath);
                    break;
                case Option.VIDEO:
                    material = (Material)Resources.Load("Materials/Video", typeof(Material));
                    NextAssetFormat = Option.VIDEO;
                    VideoStartup(NextAssetPath);
                    break;
                case Option.MASK:
                    material = (Material)Resources.Load("Materials/Mask", typeof(Material));
                    NextAssetFormat = Option.MASK;
                    MaskStartup(NextAssetPath);
                    break;
            }

            GetComponent<MeshRenderer>().sharedMaterial = material;

            material.SetFloat("_Width", pwidth);
            material.SetFloat("_Height", pheight);
            material.SetFloat("_Depth", pdepth);
            transform.localScale = dimensions;
            xscale = transform.localScale.x;
            yscale = transform.localScale.y;
            zscale = transform.localScale.z;


            GameObject go = GameObject.Find("DesktopCanvas");
            DesktopSliderManager other = (DesktopSliderManager)go.GetComponent(typeof(DesktopSliderManager));
            other.SetInitialScaleSliderValue();

        }

        if (false)
        {
            if (ThisFPSCalculator.f_Fps > 30.0)
            {
                if (Steps < 100)
                {
                    Steps = Steps + 1;
                }
            }

            if (ThisFPSCalculator.f_Fps < 30.0)
            {
                if (Steps > 12)
                {
                    Steps = Steps - 1;
                }
            }
        }



        material.SetInt("_Steps", Steps);

        // update properities based on asset format
        switch (AssetFormat)
        {
            case Option.SINGLECHANNEL:
                SingleChannelUpdate();
                break;
            case Option.MULTICHANNEL:
                MultiChannelUpdate();
                break;
            case Option.VIDEO:
                VideoUpdate(AssetPath);
                break;
            case Option.MASK:
                MaskUpdate();
                break;
        }

        material.SetFloat("_Pwidth", pwidth);
        material.SetFloat("_Pheight", pheight);
        material.SetFloat("_Pdepth", pdepth);
        //transform.localScale = dimensions;
        //Kyle crop
        material.SetFloat("_Nwidth", nwidth);
        material.SetFloat("_Nheight", nheight);
        material.SetFloat("_Ndepth", ndepth);

        //LocalScale();

        //ThisTransform.localScale = new Vector3(dimensions.x * width_scale, dimensions.y * height_scale, dimensions.z * depth_scale);


        PreviousAssetFormat = NextAssetFormat;
        PreviousAssetPath = NextAssetPath;
    }

    // Add corotine
    void SingleChannelStartup(string path)
    {
        Object TextureStack;
        TextureStack = Resources.Load(path, typeof(Texture3D));
        material.SetTexture("_Stack", (Texture3D)TextureStack);

        // set initial transform
        dimensions = calculateInitialDimensions((Texture3D)TextureStack);
    }

    void SingleChannelUpdate()
    {
        material.SetFloat("_R", R);
        material.SetFloat("_G", G);
        material.SetFloat("_B", B);

        material.SetFloat("_GlobalAttenuation", GlobalAttenuation);
        material.SetFloat("_HiddenAttenuation", HiddenAttenuation);

        material.SetFloat("_GlobalBrightness", GlobalBrightness);

        material.SetFloat("_FilterThreshold", FilterThreshold);

        material.SetColor("_Channel0Color", Channel0Color);
        material.SetColor("_Channel1Color", Channel1Color);
        material.SetColor("_Channel2Color", Channel2Color);
        material.SetColor("_Channel3Color", Channel3Color);
        material.SetColor("_Channel4Color", Channel4Color);
        material.SetColor("_Channel5Color", Channel5Color);
        material.SetColor("_Channel6Color", Channel6Color);

        material.SetFloat("_Channel0Amount", Channel0Amount);
        material.SetFloat("_Channel1Amount", Channel1Amount);
        material.SetFloat("_Channel2Amount", Channel2Amount);
        material.SetFloat("_Channel3Amount", Channel3Amount);
        material.SetFloat("_Channel4Amount", Channel4Amount);
        material.SetFloat("_Channel5Amount", Channel5Amount);
        material.SetFloat("_Channel6Amount", Channel6Amount);



    }

    void MultiChannelStartup(string path)
    {
        // load assets from input folder path
        Object[] TextureStacks;
        TextureStacks = Resources.LoadAll(path, typeof(Texture3D));

        // check for valid number of channels
        int NumChannels = TextureStacks.Length - 1;
        if (NumChannels > 127)
        {
            Debug.Log("Error: Too many channels for multi-channel stack (maximum 127)");
            return;
        }
        else
        {
            // set shader properties
            material.SetInt("_NumChannels", TextureStacks.Length - 1);

            for (int ChannelIndex = 0; ChannelIndex < TextureStacks.Length - 1; ChannelIndex++)
            {
                string ChannelProperty = "_Channel" + ChannelIndex.ToString();
                material.SetTexture(ChannelProperty, (Texture3D)TextureStacks[ChannelIndex]);
            }

            material.SetTexture("_FullStack", (Texture3D)TextureStacks[TextureStacks.Length - 1]);

            // set initial transform
            dimensions = calculateInitialDimensions((Texture3D)TextureStacks[0]);

            Texture3D tmptex3d = (Texture3D)TextureStacks[0];

            CheckPixelInTexture3D((Texture3D)TextureStacks[0]);

        }
    }

    void MultiChannelUpdate()
    {
        material.SetColor("_Channel0Color", Channel0Color);
        material.SetColor("_Channel1Color", Channel1Color);
        material.SetColor("_Channel2Color", Channel2Color);
        material.SetColor("_Channel3Color", Channel3Color);
        material.SetColor("_Channel4Color", Channel4Color);
        material.SetColor("_Channel5Color", Channel5Color);
        material.SetColor("_Channel6Color", Channel6Color);

        material.SetFloat("_Channel0Amount", Channel0Amount);
        material.SetFloat("_Channel1Amount", Channel1Amount);
        material.SetFloat("_Channel2Amount", Channel2Amount);
        material.SetFloat("_Channel3Amount", Channel3Amount);
        material.SetFloat("_Channel4Amount", Channel4Amount);
        material.SetFloat("_Channel5Amount", Channel5Amount);
        material.SetFloat("_Channel6Amount", Channel6Amount);

        material.SetFloat("_minAlpha", minAlpha);

        material.SetFloat("_GlobalAttenuation", GlobalAttenuation);
        material.SetFloat("_HiddenAttenuation", HiddenAttenuation);

        material.SetFloat("_GlobalBrightness", GlobalBrightness);

        material.SetFloat("_FilterThreshold", FilterThreshold);

        material.SetFloat("_Channel0GlobalAttenuation", Channel0GlobalAttenuation);
        material.SetFloat("_Channel1GlobalAttenuation", Channel1GlobalAttenuation);
        material.SetFloat("_Channel2GlobalAttenuation", Channel2GlobalAttenuation);
        material.SetFloat("_Channel3GlobalAttenuation", Channel3GlobalAttenuation);
        material.SetFloat("_Channel4GlobalAttenuation", Channel4GlobalAttenuation);
        material.SetFloat("_Channel5GlobalAttenuation", Channel5GlobalAttenuation);
        material.SetFloat("_Channel6GlobalAttenuation", Channel6GlobalAttenuation);

        material.SetFloat("_Channel0GlobalBrightness", Channel0GlobalBrightness);
        material.SetFloat("_Channel1GlobalBrightness", Channel1GlobalBrightness);
        material.SetFloat("_Channel2GlobalBrightness", Channel2GlobalBrightness);
        material.SetFloat("_Channel3GlobalBrightness", Channel3GlobalBrightness);
        material.SetFloat("_Channel4GlobalBrightness", Channel4GlobalBrightness);
        material.SetFloat("_Channel5GlobalBrightness", Channel5GlobalBrightness);
        material.SetFloat("_Channel6GlobalBrightness", Channel6GlobalBrightness);

        material.SetFloat("_Channel0FilterThreshold", Channel0FilterThreshold);
        material.SetFloat("_Channel1FilterThreshold", Channel1FilterThreshold);
        material.SetFloat("_Channel2FilterThreshold", Channel2FilterThreshold);
        material.SetFloat("_Channel3FilterThreshold", Channel3FilterThreshold);
        material.SetFloat("_Channel4FilterThreshold", Channel4FilterThreshold);
        material.SetFloat("_Channel5FilterThreshold", Channel5FilterThreshold);
        material.SetFloat("_Channel6FilterThreshold", Channel6FilterThreshold);

        // new gama function test part
        material.SetFloat("_GamaLowerBound", GamaLowerBound);
        material.SetFloat("_GamaUpperBound", GamaUpperBound);
        material.SetFloat("_GamaCurveValue", GamaCurveValue);

        material.SetFloat("_Channel00GamaLowerBound", Channel00GamaLowerBound);
        material.SetFloat("_Channel00GamaUpperBound", Channel00GamaUpperBound);
        material.SetFloat("_Channel00GamaCurveValue", Channel00GamaCurveValue);

        material.SetFloat("_Channel01GamaLowerBound", Channel01GamaLowerBound);
        material.SetFloat("_Channel01GamaUpperBound", Channel01GamaUpperBound);
        material.SetFloat("_Channel01GamaCurveValue", Channel01GamaCurveValue);

        material.SetFloat("_Channel02GamaLowerBound", Channel02GamaLowerBound);
        material.SetFloat("_Channel02GamaUpperBound", Channel02GamaUpperBound);
        material.SetFloat("_Channel02GamaCurveValue", Channel02GamaCurveValue);

        material.SetFloat("_Channel03GamaLowerBound", Channel03GamaLowerBound);
        material.SetFloat("_Channel03GamaUpperBound", Channel03GamaUpperBound);
        material.SetFloat("_Channel03GamaCurveValue", Channel03GamaCurveValue);

        material.SetFloat("_Channel04GamaLowerBound", Channel04GamaLowerBound);
        material.SetFloat("_Channel04GamaUpperBound", Channel04GamaUpperBound);
        material.SetFloat("_Channel04GamaCurveValue", Channel04GamaCurveValue);

        material.SetFloat("_Channel05GamaLowerBound", Channel05GamaLowerBound);
        material.SetFloat("_Channel05GamaUpperBound", Channel05GamaUpperBound);
        material.SetFloat("_Channel05GamaCurveValue", Channel05GamaCurveValue);

        material.SetFloat("_Channel06GamaLowerBound", Channel06GamaLowerBound);
        material.SetFloat("_Channel06GamaUpperBound", Channel06GamaUpperBound);
        material.SetFloat("_Channel06GamaCurveValue", Channel06GamaCurveValue);

        material.SetFloat("_AlphaTest", AlphaTest);
    }

    void VideoStartup(string path)
    {
        // set shader properties
        material.SetInt("_NumChannels", 14);

        Object Tick0ChannelAsset = new Object();
        Object Tick1ChannelAsset = new Object();
        for (int ChannelIndex = 0; ChannelIndex < 14; ChannelIndex++)
        {
            // resolve asset file paths
            string Tick0FilePath = path + "/Tick0_Channel" + ChannelIndex.ToString();
            string Tick1FilePath = path + "/Tick1_Channel" + ChannelIndex.ToString();
            string FrontBufferProperty = "FrontBuffer_Channel" + ChannelIndex.ToString();
            string BackBufferProperty = "BackBuffer_Channel" + ChannelIndex.ToString();

            // load and push assets to shader
            Tick0ChannelAsset = Resources.Load(Tick0FilePath, typeof(Texture3D));
            Tick1ChannelAsset = Resources.Load(Tick1FilePath, typeof(Texture3D));
            material.SetTexture(FrontBufferProperty, (Texture3D)Tick0ChannelAsset);
            material.SetTexture(BackBufferProperty, (Texture3D)Tick1ChannelAsset);
        }

        material.SetInt("_DisplayBuffer", 1);

        // set initial transform dimensions
        dimensions = calculateInitialDimensions((Texture3D)Tick0ChannelAsset);
    }

    void VideoUpdate(string path)
    {
        material.SetInt("_NumChannels", NumChannels);
        material.SetColor("_Channel0Color", Channel0Color);
        material.SetFloat("_Channel0Amount", Channel0Amount);

        // tick and update frame
        TimeSinceLastFrame += (Time.deltaTime * PlaybackSpeed);
        if (TimeSinceLastFrame > 0.27)
        {
            // reset time since last frame
            TimeSinceLastFrame = 0.0f;

            // wrap frame to loop video
            Tick = ++Tick % 25;

            // load next frame in the background
            int NextTick = (Tick + 1) % 25;
            string TexturePath = path + "/Tick" + NextTick.ToString() + "_Channel0";
            if (DisplayBuffer == BufferType.FRONTBUFFER)
            {
                DisplayBuffer = BufferType.BACKBUFFER;
                // switch render buffers
                material.SetInt("_DisplayBuffer", (int)DisplayBuffer);
                material.SetTexture("_FrontBuffer_Channel0", (Texture3D)Resources.Load(TexturePath, typeof(Texture3D)));
            }
            else if (DisplayBuffer == BufferType.BACKBUFFER)
            {
                DisplayBuffer = BufferType.FRONTBUFFER;
                // switch render buffers
                material.SetInt("_DisplayBuffer", (int)DisplayBuffer);
                material.SetTexture("_BackBuffer_Channel0", (Texture3D)Resources.Load(TexturePath, typeof(Texture3D)));
            }

            //Resources.UnloadUnusedAssets();

            Debug.Log("Current frame: " + Tick);
            Debug.Log("Next frame loaded: " + TexturePath);
        }
    }

    void MaskStartup(string path)
    {
        // push stack and mask assets to GPU
        string StackPath = path + "/Original";
        string MaskPath = path + "/Nucleus";
        Object StackAsset = Resources.Load(StackPath, typeof(Texture3D));
        Object MaskAsset = Resources.Load(MaskPath, typeof(Texture3D));
        material.SetTexture("_Stack", (Texture3D)StackAsset);
        material.SetTexture("_Mask", (Texture3D)MaskAsset);

        // set initial transformation
        dimensions = calculateInitialDimensions((Texture3D)StackAsset);
    }

    void MaskUpdate()
    {
        // update color and percentage to display
        material.SetColor("_Channel0Color", Channel0Color);
        material.SetFloat("_Channel0Amount", Channel0Amount);
    }

    // build mesh for shader to render image stack
    Mesh buildMesh(float width, float height, float depth)
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-width, -height, -depth),
            new Vector3(width, -height, -depth),
            new Vector3(width, height, -depth),
            new Vector3(-width, height, -depth),
            new Vector3(-width, height, depth),
            new Vector3(width, height, depth),
            new Vector3(width, -height, depth),
            new Vector3(-width, -height, depth)
        };

        int[] triangles = new int[]
        {
            0, 2, 1,
            0, 3, 2,
            2, 3, 4,
            2, 4, 5,
            1, 2, 5,
            1, 5, 6,
            0, 7, 4,
            0, 4, 3,
            5, 4, 7,
            5, 7, 6,
            0, 6, 7,
            0, 1, 6
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    Vector3 calculateInitialDimensions(Texture3D tex)
    {
        float width = 1.0f;
        float height = 1.0f;
        float depth = 1.0f;
        float texWidth = tex.width;
        float texHeight = tex.height;
        float texDepth = tex.depth;

        float scale;
        if ((scale = texWidth / texHeight) >= 1f)
        {
            width = scale;
        }
        else
        {
            height = scale;
        }

        if (AssetFormat == Option.MULTICHANNEL || AssetFormat == Option.VIDEO)
        {
            depth = 0.25f;
        }
        //depth = texDepth / 180; // generally, dividing by 180 makes for a good scale (should find a more dynamic way to do this though)

        return new Vector3(width, height, depth);
    }

    void CheckPixelInTexture3D(Texture3D tex)
    {
        float texWidth = tex.width;
        float texHeight = tex.height;
        float texDepth = tex.depth;

        Color32[] pixels = tex.GetPixels32();

        Debug.Log(pixels.Length);
    }
}