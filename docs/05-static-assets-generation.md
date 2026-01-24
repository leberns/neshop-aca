# Static assets generation with AI

The product pictures were generated with https://leonardo.ai/

Prompt examples:

```
create an image of "A 2-person heavy-duty basecamp tent built for extreme conditions.". Transparent background.

create an image of "Reinforced leather boots for technical rocky scrambles.". White image background. 1024x1024 pixels.

create an image of "Expedition-grade 75 liters backpack with modular external storage.". White image background. 1024x1024 pixels.

create an image of "Front view of expedition-grade 75 liters backpack with modular external storage.". White image background. 1024x1024 pixels.

create an image of "Minimalist hydration backpack 15 liters for peak bagging.". White image background. 1024x1024 pixels.

create an image of "Roll-top waterproof backpack for the modern explorer.". White image background. 1024x1024 pixels.
```

As a side note about the prompts:

- `transparent background` was generating any background with a low quality.
- at least with `White image background` it was generating a background that was easier to select and remove.

The images were edited with [GIMP](https://www.gimp.org/) afterwards:

- make the background transparent
  - an alpha channel was added to the images
  - the whitish background was fussily selected and deleted, making the background transparent
- the images were converted to a more efficient web image format

Images path in WebFrontend project: [product-images](../src/ShopWebl/WebFrontend/wwwroot/assets/product-images/)
