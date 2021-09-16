//Bryan Leister, August 2021
//This script creates unique images by reading textures from folders located in the Resource folder. Each folder should be
//for a certain zone, i.e hat/hair or clothing.
//
//Instructions:
//  1. Drag all of your hair/hat options into the Top folder
//  2. Repeat for every other area
//  3. There is no limit to the number of areas, but the m_images and m_paths need to be exactly the same count
//  4. All images should be the same size and transparent
//  5. Hit play and then the Make All button. Images will be written to the hard drive, depending on your system
//  6. When using the Unity Editor, the images will be by default above the Assets folder inside a folder called
//  'screenshots'
//
//The sample project on https://github.com/bryanrtboy/MemeMaker is set to run at 512px X 512px and expects 512px images
//to make pixel perfect Memes. This can be change to whatever dimension you want by changing the Preview area and Canvas
//Scaler component.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace MemeMaker.Scripts
{
    public class MakerScript : MonoBehaviour
    {
        [Tooltip("These are the images inside the Canvas UI area of the scene. Names should match exactly the names of your folders " +
                 " in a Resources folder and should not be the same as any name used in another scene (like the Demo scene).")]
        public RawImage[] m_images;
        [Tooltip("If this and the palette are null, a random background color will be picked from the palette")]
        public RawImage m_background;
        public RawImage m_floor;
        public Texture2D m_palette;

        private string[] _paths;
        private List<string> _permutations;
        private int _maxPermutationCount = 10;
        private Accessories[] _accessories;
        private GameObject _go;

        void Start()
        {
            _paths = new string[m_images.Length];
            for (int i = 0; i < m_images.Length; i++)
            {
                _paths[i] = m_images[i].name;
            }

            _accessories = new Accessories[_paths.Length];
            
            int[] maxCalculator = new int[_paths.Length];
            for (int i = 0; i < _accessories.Length; i++)
            {
                _accessories[i] = new Accessories();
                _accessories[i].m_textures = Resources.LoadAll(_paths[i], typeof(Texture2D));

                if (_accessories[i].m_textures.Length < 1)
                {
                    Debug.LogError("You need at lease one texture per folder. If you don't want to use a folder, don't add" +
                                   " the folder name to the m_paths list!");
                    Destroy(this);
                }

                maxCalculator[i] = _accessories[i].m_textures.Length;
            }

            _maxPermutationCount = CalculatePossiblePermutations(maxCalculator) / 2;
//        Debug.Log("Max permutations is " + CalculatePossiblePermutations(maxCalculator).ToString());
            GenerateRandomCharacter();
            GenerateRandomPermutationIDStrings();
        }
    
        public void GenerateRandomCharacter()
        {
            for (int i = 0; i < m_images.Length; i++)
            {
                int rand = UnityEngine.Random.Range(0, _accessories[i].m_textures.Length);
                MakeAccessory(i,rand);
            }
            if (m_background && m_palette)
                SetBackgroundColor();
        }

        int CalculatePossiblePermutations(int[] accessoryCountPerArea)
        {
            int maxPossiblePermutations = 1;
            foreach (int i in accessoryCountPerArea)
            {
                maxPossiblePermutations *= i;
            }

            return maxPossiblePermutations;
        }
        void GenerateRandomPermutationIDStrings()
        {
            _permutations = new List<string>();

            //It's advised to keep the max permutation count much less than actual possibilities,
            //otherwise this calculation could go on a really long time, so I've set the max to half of
            //what's actually possible.
            while (_permutations.Count < _maxPermutationCount)
            {
                string possiblePermutation = "";
                for (int i = 0; i < m_images.Length; i++)
                {
                    int rand = UnityEngine.Random.Range(0, _accessories[i].m_textures.Length);
                    possiblePermutation += rand.ToString();
                }

                //Check if this random combination exists already, if not add it to the list of unique ID's
                if (!_permutations.Contains(possiblePermutation))
                {
                    _permutations.Add(possiblePermutation);
                }
            }
        }

        public void MakeMemesFromPermutationStrings()
        {
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            StartCoroutine(Make(_permutations, buttons));
        }

        IEnumerator Make(List<string> list, Button[] buttons)
        {
            foreach (string permutation in list)
            {
                for (int slot = 0; slot < permutation.Length; slot++)
                {
                    int index = Int32.Parse(permutation[slot].ToString());
                    MakeAccessory(slot,index);
                }

                if (m_background && m_palette)
                    SetBackgroundColor();
                
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot("screenshots/" +permutation+".png");
            }
            yield return new WaitForEndOfFrame();
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
            }
        }

        void MakeAccessory(int slot, int index)
        {
            m_images[slot].texture = (Texture2D)_accessories[slot].m_textures[index];
        }

        void SetBackgroundColor()
        {
            int r = UnityEngine.Random.Range(0, m_palette.width);
            m_background.color = m_palette.GetPixel(r, 0);

            if (m_palette.height <= 1)
                return;
            if(m_floor)
                m_floor.color = m_palette.GetPixel(r, 1);
        }
    }

    [Serializable]
    public class Accessories
    {
        public Object[] m_textures;
    }
}
