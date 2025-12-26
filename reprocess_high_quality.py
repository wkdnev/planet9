from PIL import Image
import os

def remove_grey_background(img):
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    # Heuristic: Remove grey/white background pixels
    # Adjusting thresholds based on previous artifact analysis
    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            if a > 0:
                # Check if grey-ish (low saturation)
                diff = max(r, g, b) - min(r, g, b)
                brightness = (r + g + b) / 3.0
                
                # Artifacts were around 150-170 brightness and low diff
                # We also want to catch the white/light grey background if present
                if diff < 30 and brightness > 100:
                    # Be careful not to delete the alien itself if it's grey
                    # But usually aliens are green/red/etc.
                    pixels[x, y] = (0, 0, 0, 0)
    return img

def crop_to_content(img):
    bbox = img.getbbox()
    if bbox:
        return img.crop(bbox)
    return img

def process_high_quality():
    # 1. Alien
    print("Processing Alien from original...")
    if os.path.exists('Content/alien1_original.png'):
        img = Image.open('Content/alien1_original.png')
        img = remove_grey_background(img)
        img = crop_to_content(img)
        
        # Resize to 60px width
        target_width = 60
        w_percent = (target_width / float(img.size[0]))
        h_size = int((float(img.size[1]) * float(w_percent)))
        img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
        
        img.save('Content/alien1.png')
        print(f"Saved high-quality Content/alien1.png ({img.size})")
    
    # 2. Player Ship (Moving)
    print("Processing Player Ship from original...")
    if os.path.exists('Content/player_ship_original.png'):
        img = Image.open('Content/player_ship_original.png')
        # User said this one has transparent background, but let's crop just in case
        img = crop_to_content(img)
        
        # Resize to 70px width
        target_width = 70
        w_percent = (target_width / float(img.size[0]))
        h_size = int((float(img.size[1]) * float(w_percent)))
        img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
        
        img.save('Content/player_ship.png')
        print(f"Saved high-quality Content/player_ship.png ({img.size})")

        # 3. Player Ship (Idle) - Recreating from original moving ship
        # We lost the high-res idle image, so we approximate by cropping the bottom of the moving ship
        print("Generating Idle Ship from original...")
        img_idle = Image.open('Content/player_ship_original.png')
        img_idle = crop_to_content(img_idle)
        
        # Crop bottom 15% (assuming thrusters are at the bottom)
        w, h = img_idle.size
        # Crop rect: left, top, right, bottom
        img_idle = img_idle.crop((0, 0, w, int(h * 0.85)))
        
        # Resize to match width
        w_percent = (target_width / float(img_idle.size[0]))
        h_size = int((float(img_idle.size[1]) * float(w_percent)))
        img_idle = img_idle.resize((target_width, h_size), Image.Resampling.LANCZOS)
        
        img_idle.save('Content/player_ship_non_move.png')
        print(f"Generated high-quality Content/player_ship_non_move.png ({img_idle.size})")

if __name__ == "__main__":
    process_high_quality()
