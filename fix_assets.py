from PIL import Image

def remove_grey_artifacts(img, threshold=30):
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            if a > 0:
                # Check if grey-ish
                diff = max(r, g, b) - min(r, g, b)
                brightness = (r + g + b) / 3.0
                
                # Artifacts seem to be in 150-170 range and low saturation
                if diff < 20 and 100 < brightness < 190:
                    pixels[x, y] = (0, 0, 0, 0)
    return img

def crop_to_content(img):
    bbox = img.getbbox()
    if bbox:
        return img.crop(bbox)
    return img

def process_alien():
    print("Processing Alien...")
    img = Image.open('Content/alien1.png')
    
    # 1. Remove artifacts
    img = remove_grey_artifacts(img)
    
    # 2. Crop to content
    img = crop_to_content(img)
    
    # 3. Resize to larger size (e.g. 60px width)
    target_width = 60
    w_percent = (target_width / float(img.size[0]))
    h_size = int((float(img.size[1]) * float(w_percent)))
    img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
    
    img.save('Content/alien1.png')
    print(f"Saved alien1.png at {img.size}")

def process_ship(filename, target_width=70):
    print(f"Processing {filename}...")
    img = Image.open(filename)
    
    # 1. Crop to content
    img = crop_to_content(img)
    
    # 2. Resize
    w_percent = (target_width / float(img.size[0]))
    h_size = int((float(img.size[1]) * float(w_percent)))
    img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
    
    img.save(filename)
    print(f"Saved {filename} at {img.size}")

if __name__ == "__main__":
    process_alien()
    process_ship('Content/player_ship.png')
    process_ship('Content/player_ship_non_move.png')
