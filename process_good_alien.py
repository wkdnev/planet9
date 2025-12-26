from PIL import Image, ImageDraw
import math

def color_distance(c1, c2):
    return math.sqrt(sum((a - b) ** 2 for a, b in zip(c1[:3], c2[:3])))

def is_grey(c):
    # Check if the pixel is grey-ish (low saturation)
    r, g, b = c[:3]
    diff = max(r, g, b) - min(r, g, b)
    return diff < 20

def process_alien():
    print("Processing alien_good.png...")
    img = Image.open('Content/alien_good.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    # 1. Background Removal using Flood Fill
    # We'll start from multiple points on the border to catch the background
    queue = []
    visited = set()
    
    # Add border pixels to queue
    for x in range(width):
        queue.append((x, 0))
        queue.append((x, height-1))
    for y in range(height):
        queue.append((0, y))
        queue.append((width-1, y))
        
    # Filter queue to only include unique points
    queue = list(set(queue))
    
    # We need a reference color, but the background varies.
    # We will assume anything "grey-ish" connected to the border is background.
    # The corner pixels gave us a range of ~150-180 brightness.
    
    dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)]
    
    while queue:
        x, y = queue.pop(0)
        
        if (x, y) in visited:
            continue
        visited.add((x, y))
        
        r, g, b, a = pixels[x, y]
        
        # Check if this pixel looks like background
        # Criteria: Grey-ish AND within brightness range of observed background
        brightness = (r + g + b) / 3.0
        diff = max(r, g, b) - min(r, g, b)
        
        # Expanded range based on previous analysis (146 to 178)
        # Let's be generous: 100 to 200 brightness, low saturation
        if diff < 30 and 100 < brightness < 210:
            # It is background, make transparent
            pixels[x, y] = (0, 0, 0, 0)
            
            # Add neighbors
            for dx, dy in dirs:
                nx, ny = x + dx, y + dy
                if 0 <= nx < width and 0 <= ny < height:
                    queue.append((nx, ny))
    
    # 2. Crop to content
    bbox = img.getbbox()
    if bbox:
        img = img.crop(bbox)
        print(f"Cropped to {img.size}")
    
    # 3. Resize
    # Target width 60px
    target_width = 60
    w_percent = (target_width / float(img.size[0]))
    h_size = int((float(img.size[1]) * float(w_percent)))
    
    # Use LANCZOS for best downscaling quality
    img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
    
    # 4. Sharpen slightly? 
    # Sometimes downscaling makes things blurry. 
    # Let's just save it first.
    
    img.save('Content/alien1.png')
    print(f"Saved Content/alien1.png ({img.size})")

if __name__ == "__main__":
    process_alien()
