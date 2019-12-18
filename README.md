# Unity-based, GPU-accelerated Ray-tracing Scheme for Real-time Rendering of 3D Simulation Data

## Project Description
Visualization tools are important for analyzing and communicating scientific data of all forms. This data is often multi-dimensional and can be visualized as everything from tables to graphs, charts, images, and cartoons. Simulation data, specifically, is an area where visualization is particularly valuable. It is much easier for researchers and the public to understand a group of particles as a visual figure than as a list of coordinates (see figure below). However, in most applications simulation data is calculated in 3 dimensions, and the generation of such 3D figures for visualization is, unfortunately, non-trivial. 

![Unity screenshot](/application-new.png)

One reason for this is that most physical displays, from academic papers to computer monitors, are 2D. As a result, the creation of a digital 3D figure must be reformulated from a plotting task to a rendering task in which the 3D data is projected onto a 2D plane in a way that preserves a sense of depth. With sufficient time, this task is easy enough and 3D data can be trivially rendered upon a 2D plane for a given perspective. However, occlusion and ambiguities in this sense of depth mean that one perspective is often not sufficient to visualize an entire 3D figure. An ideal software for 3D viewing on a 2D screen therefore must allow the viewer to shift perspectives such that the updated 2D projection is calculated and displayed at real-time, interactive speed, resolving and occlusion and ambiguity by parallax. Calculating such projections at real-time speeds is computationally intensive and therefore requires the integration of high performance methods to improve performance. Towards this end, this project presents a GPU-based software solution for interactively rendering 3D simulation data.

## Simulation Methods and Algorithms
Many GPU-based solutions for rendering 3D simulation data exist (Beyer, 2014). The challenge is deploying a solution in an interactive environment with the constraint of maintaining real-time rendering speed. The first step is creating software for the interactive environment. While it is possible, and perhaps optimal in some cases, to create interactive environments from scratch, it is often preferable to leverage existing game engines for easier cross-platform compatibility and implementation. This project leverages the game engine Unity. Using this game engine, objects can be instantiated within game space and rendered automatically with simple controls for user interactivity. For the case of simulation data, this means a software solution can simply instantiate spheres at all the proper positions in game space and leave the rest to the engine. The issue is that such data representations are quickly limited by the amount of sphere surfaces that can be drawn by the CPU (see figure below). Therefore, it is logical to explore the integration of GPU-based methods in game environments to increase performance capability.

![Unity screenshot](/problem.png)

In order to use the GPU for rendering, our concept is to represent a list of 3D coordinates from simulation data as a single 3D image texture instead of individual spheres (see data_formatting.ipynb). This method is popular in biological image rendering (see figure below). Using this image-based representation, we can then use the game engine to ingest the image as a 3D texture (see SingleChannelASsetBuilder.cs), load the texture as the material of a simple object (see VolumeRenderer.cs), and then write shader code that dynamically performs GPU accelerated raycasting to project 2D images from the perspective of the viewer (.shader file not provided as it is under a provisional patent). The advantage here is that regardless of the number of coordinates, the CPU only has to worry about tracking one object, the volume renderer, while all projection calculations are offloaded to the GPU.

![Unity screenshot](/solution.png)

## Results

A qualitative result of our technique deployed on 3D nanocarbon data can be viewed in the video below. As you can see our GPU-based solution achieves interactive rendering speed in the Unity engine. Quantitatively, it would be useful. 

4D possible, perhaps not preferable. 

## Future directions

## References

https://people.eecs.berkeley.edu/~driscoll/cs267/hw0/pdfs/KiChow.pdf

Beyer, Johanna, Markus Hadwiger, and Hanspeter Pfister. 2014. “A Survey of GPU-Based Large-Scale Volume Visualization.” In Eurographics Conference on Visualization (EuroVis)(2014). IEEE Visualization and Graphics Technical Committee (IEEE VGTC). http://people.seas.harvard.edu/~jbeyer/files/documents/beyer_star_large_volren_14.pdf.
