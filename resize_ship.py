from PIL import Image
import os
import shutil

def resize_ship():
    src = 'Content/player_ship.png'
    backup = 'Content/player_ship_original.png'
    
    # Backup if not exists
    if not os.path.exists(backup):
        shutil.copy2(src, backup)
        print(f"Backed up original to {backup}")
    
    img = Image.open(src)
    print(f"Original size: {img.size}")
    
    # Target width 64px
    target_width = 64
    w_percent = (target_width / float(img.size[0]))
    h_size = int((float(img.size[1]) * float(w_percent)))
    
    # Resize using LANCZOS for quality
    img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
    
    img.save(src)
    print(f"Resized to: {img.size}")

if __name__ == "__main__":
    resize_ship()
