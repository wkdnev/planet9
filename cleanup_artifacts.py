from PIL import Image

def is_background(r, g, b, a):
    if a == 0: return False # Already transparent
    
    # Check for low saturation (grey-ish)
    diff = max(r, g, b) - min(r, g, b)
    
    # Check for brightness range (based on observed artifacts ~140-165)
    brightness = (r + g + b) / 3.0
    
    # Heuristics
    is_grey = diff < 20
    is_in_range = 130 < brightness < 180
    
    return is_grey and is_in_range

def cleanup_artifacts():
    img = Image.open('Content/player_ship_non_move.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    count = 0
    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            if is_background(r, g, b, a):
                pixels[x, y] = (0, 0, 0, 0)
                count += 1
                
    img.save('Content/player_ship_non_move.png')
    print(f"Removed {count} artifact pixels.")

if __name__ == "__main__":
    cleanup_artifacts()
