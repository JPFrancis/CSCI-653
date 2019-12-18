# Unity-based, GPU-accelerated Ray-tracing Scheme for Real-time Rendering of 3D Simulation Data

## Project Description
Visualization tools are important for analyzing and communicating scientific data of all forms. This scientific data is often multi-dimensional and can be visualized as everything from tables to graphs, charts, images, and cartoons. Simulation data, specifically, is an area where visualization is particularly valuable. It is much easier for researchers and the public to understand a group of particles as a visual figure than as a list of coordinates. However, in most applications simulation data is calculated in 3 dimensions, and the generation of such 3D figures for visualization is, unfortunately, non-trivial. 

One reason for this is that most physical displays, from academic papers to computer monitors, are 2D. As a result, the creation of a digital 3D figure must be reformulated from a plotting task to a rendering task in which the 3D data is projected onto a 2D plane in a way that preserves a sense of "depth". With sufficient time, this task is easy enough and 3D data can be trivially rendered upon a 2D plane for a given perspective. However, occlusion and ambiguities in this sense of depth mean that one perspective is often not sufficient to visualize an entire 3D figure. An ideal software for 3D viewing on a 2D screen must therefore allow the viewer to shift perspectives such that the updated 2D projection is calculated and displayed at real-time, interactive speed and occlusion and ambiguity are resolved by parallax. This project presents a GPU-based software solution for such interactive rendering of 3D simulation data.

## Simulation Methods and Algorithms
Many GPU-based solutions for rendering 3D simulation data exist (Beyer, 2014). The challenge is deploying a solution in an interactive environment with the constraint of real-time speed. The first step is creating such an interactive environment in software. While it is possible, and perhaps optimal in some cases, to create interactive environments from scratch, it is often preferable to leverage exiting game engines for easier cross-platform compatibility and implementation. This project leverages the game engine Unity. Using this game engine, objects can be instantiated within a game space and rendered automatically with simple controls for user interactivity. In the case of simulation data, this means a software solution can simply institute spheres at all the proper positions in game space and leave the rest to the engine. The issue with this is that it is CPU-driven and quickly limited by the amount of sphere surfaces that can be drawn. Therefore, it is logical to explore the integration of GPU-based methods in game environments to increase performance capability.

The 

Implementing a GPU-accelerated rendering scheme in a game-engine is challenging as it requires the implementation of ray-tracing  


Traditionally, ___. However, the key idea here is presents an alternative. Treat it as an image in the hopes that. Conversion. Shader. Display. 

![Unity screenshot](/problem.png)

Represent coordinate information as an image where each atom is a pixel point.
Use ray tracing with maximum intensity projection via customized game-engine shader (leverages GPU).
![Unity screenshot](/solution.png)

## Results
![Unity screenshot](/application.png)
![Unity screenshot](/img.png)

Qualitative results are here. Quantitative results could be formulated as follows. Additionally, artifacts -- can be addressed. 4D possible, perhaps not preferable. 

## References

https://people.eecs.berkeley.edu/~driscoll/cs267/hw0/pdfs/KiChow.pdf

Beyer, Johanna, Markus Hadwiger, and Hanspeter Pfister. 2014. “A Survey of GPU-Based Large-Scale Volume Visualization.” In Eurographics Conference on Visualization (EuroVis)(2014). IEEE Visualization and Graphics Technical Committee (IEEE VGTC). http://people.seas.harvard.edu/~jbeyer/files/documents/beyer_star_large_volren_14.pdf.
