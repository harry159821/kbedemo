using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class TexturePostprocessor : AssetPostprocessor
{
    static TextureImporterPlatformSettings _android_rgba;
 static   TextureImporterPlatformSettings android_rgba
        {
        get
        {
            if (_android_rgba == null)
            {
                _android_rgba = new TextureImporterPlatformSettings();
                _android_rgba.allowsAlphaSplitting = true;                
                _android_rgba.compressionQuality = 50;
                _android_rgba.crunchedCompression = true;
                _android_rgba.format = TextureImporterFormat.ETC2_RGBA8;
                _android_rgba.textureCompression = TextureImporterCompression.Compressed;
            }
            return _android_rgba;
        }
        }
    static TextureImporterPlatformSettings _android;
    static TextureImporterPlatformSettings android
    {
        get
        {
            if (_android == null)
            {
                _android = new TextureImporterPlatformSettings();
                _android.allowsAlphaSplitting = true;
                _android.compressionQuality = 50;
                _android.crunchedCompression = true;
                _android.format = TextureImporterFormat.ETC2_RGB4;
                _android.textureCompression = TextureImporterCompression.Compressed;
            }
            return _android;
        }
    }

    static TextureImporterPlatformSettings _ios;

    static TextureImporterPlatformSettings ios
    {
        get
        {
            if (_ios == null)
            {
                _ios = new TextureImporterPlatformSettings();
                _ios.allowsAlphaSplitting = true;
                _ios.compressionQuality = 50;
                _ios.crunchedCompression = true;
                _ios.format = TextureImporterFormat.PVRTC_RGBA4;
                _ios.textureCompression = TextureImporterCompression.Compressed;
            }
            return _ios;
        }
    }
    static TextureImporterPlatformSettings _win;

    static TextureImporterPlatformSettings win
    {
        get
        {
            if (_win == null)
            {
                _win = new TextureImporterPlatformSettings();
                _win.allowsAlphaSplitting = true;
                _win.compressionQuality = 50;
                _win.crunchedCompression = true;
                _win.format = TextureImporterFormat.DXT5;
                _win.textureCompression = TextureImporterCompression.Compressed;
            }
            return _win;
        }
    }

    void OnPreprocessTexture()
    {
        
     
                var ti = assetImporter as TextureImporter;
               
                if (assetPath.Contains("_lightmap") == true)
                {
                    ti.maxTextureSize = 1024;                   
                    ti.textureType = TextureImporterType.Lightmap;
                }
                 ti.textureShape = TextureImporterShape.Texture2D;
                ti.mipmapEnabled = false;
                if (assetPath.Contains("anim/mainRole"))
                {
                    ti.isReadable = true;
                }

                if (assetPath.Contains("_rgba") || assetPath.Contains("ui/") )
                {
                    ti.SetPlatformTextureSettings(android_rgba);
                    
                }
                else
                {
                    ti.SetPlatformTextureSettings(android);
                   
                }
        ti.SetPlatformTextureSettings(win);
        ti.SetPlatformTextureSettings(ios);
    }
  
  
}