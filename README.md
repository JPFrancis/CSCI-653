# Unity-based, GPU-accelerated Ray-tracing Scheme for Real-time Rendering of 3D Simulation Data

## Project Description
Visualization tools are important for analyzing and communicating scientific data of all forms. This data is often multi-dimensional and can be visualized as everything from tables to graphs, charts, images, and cartoons. Simulation data, specifically, is an area where visualization is particularly valuable. It is much easier for researchers and the public to understand a group of particles as a visual figure than as a list of coordinates (see figure below). In most applications, however, simulation data is calculated in 3 dimensions, and the generation of 3D figures for visualization is, unfortunately, non-trivial. 

![Unity screenshot](/application_new.png)

One reason for this is that most physical displays, from academic papers to computer monitors, are 2D. As a result, the creation of a digital 3D figure must be reformulated from a plotting task to a rendering task in which the 3D data is projected onto a 2D plane in a way that preserves a sense of depth. With sufficient time, this task is straightforward and 3D data can be rendered upon a 2D plane faithfully for a given perspective. However, occlusion and ambiguities in this sense of depth mean that one perspective is often not sufficient to visualize an entire 3D figure. An ideal software for 3D viewing on a 2D screen therefore must allow the viewer to shift perspectives such that the updated 2D projection is calculated and displayed at real-time, interactive speed, resolving occlusion and ambiguity via parallax. Calculating such projections at real-time speeds is computationally intensive and therefore requires the integration of high performance computing methods to achieve sufficient performance with large datasets. Towards this end, this project presents a GPU-based software solution for interactively rendering 3D simulation data.

## Simulation Methods and Algorithms
Many GPU-based solutions for rendering 3D simulation data exist (Beyer, 2014). The challenge is deploying a solution in an interactive environment with the constraint of maintaining real-time rendering speed. The first step is creating an interactive environment in software. In some cases it is perhaps optimal to create interactive environments "from scatch" with low level graphics code, however, it is often preferable to leverage existing high-level game engines for easier cross-platform compatibility and implementation. This project leverages the game engine Unity. Using this game engine, objects can be easily instantiated within game space and rendered automatically alongside simple controls for user input and interactivity. In the case of simulation data, this means a software solution can simply instantiate spheres for all the coordinates at all the proper positions in game space and leave the rendering and interactivity to the engine. The issue is that such baseline methods are limited by the amount of sphere surfaces that can be drawn by the CPU (see figure below). Therefore, it is logical to explore the integration of more rapid GPU-based methods in game environments to increase performance capability.

![Unity screenshot](/problem.png)

In order to use the GPU for rendering, our concept is to represent a list of 3D simulation coordinates as a cohesive 3D image texture instead of individual spheres (see data_formatting.ipynb). Rendering 3D images is popular in biological image rendering applications on which this work is based (see figure below). Using this image-based representation, we can then write game engine scripts to ingest the image as a 3D texture (see SingleChannelASsetBuilder.cs), set the texture as the material of a single primitive object (see VolumeRenderer.cs), and then write shader code as an interface to the GPU to dynamically ray cast, calculate, and project a 2D image from the perspective of the viewer (.shader file not provided as it is under a provisional patent). The advantage here is that regardless of the number of coordinates, the CPU only has to worry about tracking one object, the volume renderer, while all projection calculations are offloaded to the GPU.

![Unity screenshot](/solution.png)

## Results
A qualitative result of our technique is deployed on 3D nanocarbon simulation data with 16,996 coordinates (see nanocarbon.txt original and nanocarbon.nii converted). A screen recording can be viewed [here](https://youtu.be/X4H1Yx9Si7E). Broadly, our GPU-based solution achieves interactive rendering speed in the Unity engine. Quantitatively, and for publication purposes, it will be useful to explore how frame rate, GPU utilization, and CPU utilization compare between our method and a baseline method as the number of coordinate points increases. 

It should also be mentioned though, that unlike the baseline method, there are some noticeable visual distortions on the edges of the volume that are an artifact of the ray tracing calculation on the corner of the primitive surface. This is an area that warrants further development and is particularly apparent in simulation data sets, where the data is relatively sparse and granular as compared to biology.

## Future Directions
Our solution is currently deployed on a local machine using only one GPU, as is typical in most gaming workstations. However, the ray casting procedure is a parallelizable process. There could be an opportunity to deploy our technique on a server using multiple GPUs, however the technical details of leveraging the Unity game engine on a distributed stack requires further investigation.

Additionally, most simulation data is not only 3D but also contains a time dimension. Accordingly, we tested our technique on 3D time series data of graphene and the results can be viewed [here](https://youtu.be/lXp3FzCnnyM). Unfortunately, this application did not perform as well as the application with static 3D data. This is because, although the GPU handles the projection calculations, the CPU still must be used to load and offload the 3D textures for each individual timepoint as the simulation progresses. Our method therefore does not offer any computational gains with time series data compared to the standard method of translating individual spheres in space. 

Luckily, there are many solutions that have been developed for video streaming and we could explore the use of MPEG frame compression, for a start, to reduce the overhead of updating 3D textures over time and improve this performance.


## References

Beyer, Johanna, Markus Hadwiger, and Hanspeter Pfister. 2014. “A Survey of GPU-Based Large-Scale Volume Visualization.” In Eurographics Conference on Visualization (EuroVis)(2014). IEEE Visualization and Graphics Technical Committee (IEEE VGTC). http://people.seas.harvard.edu/~jbeyer/files/documents/beyer_star_large_volren_14.pdf.

https://people.eecs.berkeley.edu/~driscoll/cs267/hw0/pdfs/KiChow.pdf
