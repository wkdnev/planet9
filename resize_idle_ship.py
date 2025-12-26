from PIL import Image
import os
import shutil

def resize_idle_ship():
    src = 'Content/player_ship_non_move.png'
    ref = 'Content/player_ship.png'
    
    if not os.path.exists(src):
        print(f"Error: {src} not found.")
        return

    # Get reference size
    ref_img = Image.open(ref)
    target_size = ref_img.size
    print(f"Target size from reference: {target_size}")
    
    img = Image.open(src)
    print(f"Original idle size: {img.size}")
    
    if img.size != target_size:
        # Resize using LANCZOS for quality
        img = img.resize(target_size, Image.Resampling.LANCZOS)
        img.save(src)
        print(f"Resized {src} to: {img.size}")
    else:
        print("Size matches, no resize needed.")

if __name__ == "__main__":
    resize_idle_ship()
