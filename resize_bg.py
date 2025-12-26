from PIL import Image
import os
import shutil

def resize_bg():
    src = 'Content/level1_bg.png'
    backup = 'Content/level1_bg_original.png'
    
    if not os.path.exists(backup):
        shutil.copy2(src, backup)
    
    img = Image.open(src)
    # Resize to 1280x720
    img = img.resize((1280, 720), Image.Resampling.LANCZOS)
    img.save(src)
    print(f"Resized background to 1280x720")

if __name__ == "__main__":
    resize_bg()
