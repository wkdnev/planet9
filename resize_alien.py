from PIL import Image
import os
import shutil

def resize_alien():
    src = 'Content/alien1.png'
    backup = 'Content/alien1_original.png'
    
    if not os.path.exists(backup):
        shutil.copy2(src, backup)
    
    img = Image.open(src)
    # Resize to width 48, keeping aspect ratio
    target_width = 48
    w_percent = (target_width / float(img.size[0]))
    h_size = int((float(img.size[1]) * float(w_percent)))
    
    img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
    img.save(src)
    print(f"Resized alien to {img.size}")

if __name__ == "__main__":
    resize_alien()
