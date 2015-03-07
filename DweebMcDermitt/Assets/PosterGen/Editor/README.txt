For technical support, contact: tmfixesandsuggestions@gmail.com

The utility is located in: GameObject->CreateOther->Posters (should be at or near the bottom)

From top to bottom, the settings are:

Area of each poster: The target area for each poster. They may not all be this area if "Limit dimensions" is checked.

Limit Dimensions: If checked, the poster's maximum width and height will be clamped at the "Maximum dimensions" value, even if it results in the poster having a smaller area than the "Area of each poster" field.

Overlap posters: Whether or not to translate the posters along a plane as they are being generated. If this is checked, the posters will all appear on top of each other. If "Overlap posters" is not checked, the "Space between" option can be set. "Space between" sets the distance between each poster.

Lift posters up by: The amount to lift all of the posters above the floor, or in the case of the XZ plane from one of the walls.

Fade out posters: Creates a fading effect between the edges of the posters and the wall at the expense of having to generate new textures to facilitate the fade out. This is because the fade out on the edges is from alpha blending, so a custom texture is required with its own alpha channel. All of these textures are exported to "PosterGenImages" under the Asset folder. The amount of blend created by this effect is determined by "Width of fade".

Create posters on:
(XY plane, etc.)
Determines which plane you want to create your posters on. There is also the option to create them on the floor if you'd prefer mats instead.

Okay, those were fairly self-explanatory, right? So what is the weird S thing below that?
It's the material you will apply to your poster! Play around with this as you'd like, you can also set the texture parameters from any of your loaded textures with the fields below. The "Tile other textures by" field determines how much to tile any non-diffuse textures by, useful for detail and normal maps. Oh, these maps will already be rescaled to the proportions of your posters, so no problems there.

To create posters, you must first select either a single image, a group of images, or an entire directory of images from the Asset pane. Then, press the "Select images from the asset pane and click here" button and your posters should be generated either on top of each other or in a straight line.