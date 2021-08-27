![Sample Animation](sample2.gif)

# MemeMaker
A Unity project to generate unique 2D memes based on a background and 4 or more accesory zones. The script loads however many images are in each sub-folder of the Resources folder. It then randomly picks either one texture or no texture from each zone. By having 10 items in each of the 4 zones it is possible to generate 10,000 unique memes.

If a background and background palette is specified, each meme gets a random background color based off the palette, otherwise the background will be the same for all of them. It would be easy to extend that to other items of the base character, like the face or clothing base. Just replicate the code used for the background to whatever other part you wish.

## Instructions

This script creates unique images by reading textures from folders located in the Resource folder. Each folder should be
for a certain zone, i.e hat/hair or clothing.

 1. Drag all of your hair/hat options into the Top folder
 2. Repeat for every other area
 3. There is no limit to the number of areas, but the m_images and m_paths need to be exactly the same count
 4. All images should be the same size and transparent
 5. Hit play and then the Make All button. Images will be written to the hard drive, depending on your system
 6. When using the Unity Editor, the screenshots will written to disk above the Assets folder inside a folder called 'screenshots'

This example scene is set to run at 512px X 512px and expects 512px images to make pixel perfect Memes. This can be change to whatever dimension you want by changing the Preview area and Canvas Scaler component.

When using a background palette, make sure it's import setting is set to Read/write in Unity.
