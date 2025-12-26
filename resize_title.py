from PIL import Image
import os
import shutil

def resize_title():
    src = 'Content/title_screen.png'
    backup = 'Content/title_screen_original.png'
    
    if not os.path.exists(backup):
        shutil.copy2(src, backup)
    
    img = Image.open(src)
    # Resize to 1280x720
    img = img.resize((1280, 720), Image.Resampling.LANCZOS)
    img.save(src)
    print(f"Resized title screen to 1280x720")

if __name__ == "__main__":
    resize_title()
