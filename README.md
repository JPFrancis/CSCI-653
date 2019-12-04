# CSCI-653

## Project Description

Rendering 3D biological images requires a representation of depth and transparency within the sample. This visual result is most often achieved through maximum intensity projection.
Ray tracing is a crucial to maximum intensity projection as each pixel result is calculated as a sum of the brightest pixels along its line of sight. The ray tracing calculation, however,
is computationally expensive. As a result rendering biological images in real-time 3D environments requires trade-offs between ray tracing sampling interval
and performance. In other words, the ray trace skips pixels to maintain reasonable playback speeds and sacrifices image quality. Ideally this would not be the case.

## Simulation Methods and Algorithms

In the ray tracing problem, each ray is independently calculated and there is a good opportunity for parallelization over multiple GPUs.
Our application of interest, Microscape, is built in Unity and the ray tracing is performed on a local GPU. We are deploying this application
on the cloud and in this process it would seem that we would be able to split up the problem and leverage multiple GPUs on the server to calculate the aggregate 4k image render.
The unknown would be whether Unity functions can be modified using more specific CUDA commands.

## Expected Results

Currently, our rays can only calculate ~50 steps before performance lags. Each addition of a GPU could conceivably double that number
and we would hope to see that a 2 GPU implementation could perform the same (measured in fps) at 100 steps as a 1 GPU implementation at 50 steps.

## References

https://people.eecs.berkeley.edu/~driscoll/cs267/hw0/pdfs/KiChow.pdf

## Images

![alt text](https://raw.githubusercontent.com/jpfrancis/CSCI-653/img.png)
