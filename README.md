# Spectra Hunt

Submission to Acerola Jam 0 Game jam.


[Executable download](https://github.com/BuyMyBeard/Spectra-Hunt/releases/tag/v1.0.0) **(Recommended, better visuals)**

Or [Play in the browser](https://buymybeard.itch.io/spectra-hunt)

![thumbnail-v4](https://github.com/BuyMyBeard/Spectra-Hunt/assets/95039323/52f2975a-f488-42a3-a175-ed9e2db40649)

This jam was an occasion for me to delve into rigging models with weight painting, animating quadrupeds, playing with VFX Graph, doing shader magic, and making cutscenes with the Unity Timeline.

I am really happy of the visuals of the game, with cute low-poly flat-shaded 3d graphics, dynamic clouds that cast moving shadows on the map, and water shader with realtime planar reflections.

This was also an occasion to practice animation, having me animate hare and fox movement animations. I incorporated procedural animations to the fox to make him adjust his rotation to the ground, and turn his body when turning.

The biggest challenge of the jam was creating the rainbow split visual effect, using the power of the stencil buffer and the render queue to display 3d models stacked on the same plane as the view angle one behind of the other without z-fighting.
