UnityURP.Outline
===
![](https://img.shields.io/badge/unity-6.0.20%2B-blue.svg)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-orange.svg)](http://makeapullrequest.com)

<br>

![image](https://github.com/user-attachments/assets/56c90ecc-9718-4be7-967a-3b1f619bbd68)

## Description

Requires Unity 6.0.20 or higher.
Compatible with Universal Render Pipeline.<br>

This repository is a fork of [Per-Object_Outline_RenderGraph_RendererFeature_Example](https://github.com/Unity-Technologies/Per-Object_Outline_RenderGraph_RendererFeature_Example), aiming to improve it and create a production ready solution.<br>

UnityURP.Outline create a soft object-outline effect in Universal Render Pipeline - Unity 6+ as a Render Feature using RenderGraph and RenderPasses. Allows you to specify individual Renderers instead of filtering them from the entire scene as would be done with DrawRendererList.<br>

## Usage

First add the `Blurred Buffer Ourline Render Feature` to your `Universal Renderer Data`, the needed shaders will be auto picked:

![image](https://github.com/user-attachments/assets/128789ec-6328-45a8-9f86-60707a4f6da0)

Then you will need to create a `OutlineMaterial`, wich let you set the color and the size of the outline. You can create and use any number of `OutlineMaterial`, but each in use will produce a render pass, so having too many of them could be a performance issue.

<img width="796" height="623" alt="image" src="https://github.com/user-attachments/assets/6e3a8e27-9f0f-48d2-aaf0-6e5bdeb8414a" />

<img width="590" height="129" alt="image" src="https://github.com/user-attachments/assets/1d842f91-48fa-479a-b1aa-71e042364b4d" />

Finally to draw the outline of a renderer, add the `OutlineRenderer` component to the target GameObject, it will require the `OutlineMaterial` to use and the list of renderers to outline. 

<img width="595" height="192" alt="image" src="https://github.com/user-attachments/assets/9906400d-6183-489c-9d85-8c30d2ab59c2" />

The list of renderers will be auto-populated at the moment you add the component with the GameObject`s childrens rendereres. You can force the auto-population in the component's context menu: 

<img width="592" height="346" alt="image" src="https://github.com/user-attachments/assets/e8d920d1-32e3-451f-93a3-f29267dfe0c5" />
