from PIL import Image, ImageEnhance, ImageFilter

def process_alien_enhanced():
    print("Processing alien_good.png with eye enhancement...")
    img = Image.open('Content/alien_good.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    # 1. Background Removal (Same as before, but careful)
    # We'll use a flood fill from corners
    queue = []
    visited = set()
    
    # Add border pixels
    for x in range(width):
        queue.append((x, 0))
        queue.append((x, height-1))
    for y in range(height):
        queue.append((0, y))
        queue.append((width-1, y))
    
    queue = list(set(queue))
    dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)]
    
    while queue:
        x, y = queue.pop(0)
        if (x, y) in visited: continue
        visited.add((x, y))
        
        r, g, b, a = pixels[x, y]
        
        # Background criteria
        brightness = (r + g + b) / 3.0
        diff = max(r, g, b) - min(r, g, b)
        
        if diff < 30 and 100 < brightness < 210:
            pixels[x, y] = (0, 0, 0, 0)
            for dx, dy in dirs:
                nx, ny = x + dx, y + dy
                if 0 <= nx < width and 0 <= ny < height:
                    queue.append((nx, ny))

    # 2. Enhance Eyes (Green pixels)
    # We iterate through all pixels, find green ones, and make them brighter/more vivid
    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            if a > 0:
                # Check for green
                if g > r + 10 and g > b + 10:
                    # It's greenish. Boost it!
                    # Make it neon green
                    new_g = min(255, int(g * 1.5) + 50)
                    new_r = int(r * 0.8)
                    new_b = int(b * 0.8)
                    pixels[x, y] = (new_r, new_g, new_b, a)

    # 3. Crop
    bbox = img.getbbox()
    if bbox:
        img = img.crop(bbox)
    
    # 4. Resize
    target_width = 60
    w_percent = (target_width / float(img.size[0]))
    h_size = int((float(img.size[1]) * float(w_percent)))
    
    img = img.resize((target_width, h_size), Image.Resampling.LANCZOS)
    
    # 5. Sharpen
    img = img.filter(ImageFilter.UnsharpMask(radius=2, percent=150, threshold=3))
    
    img.save('Content/alien1.png')
    print(f"Saved enhanced Content/alien1.png ({img.size})")

if __name__ == "__main__":
    process_alien_enhanced()
