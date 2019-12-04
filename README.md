# CSCI-653 Final Project: GPU-accelerated ray tracing for volumetric rendering of simulation data

## Project Description
Rendering simulation data for interactive viewing is limited by the amount of particles. Must draw every sphere at every frame. Each sphere requires an iso surface.
![Unity screenschot](/problem.png)

## Simulation Methods and Algorithms

Represent coordinate information as an image where each atom is a pixel point.
Use ray tracing with maximum intensity projection via customized game-engine shader (leverages GPU).
![Unity screenschot](/solution.png)

## Results
![Unity screenschot](/application.png)
![Unity screenschot](/img.png)

## References

https://people.eecs.berkeley.edu/~driscoll/cs267/hw0/pdfs/KiChow.pdf
