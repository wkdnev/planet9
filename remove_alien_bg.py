from PIL import Image, ImageDraw
import math

def color_distance(c1, c2):
    return math.sqrt(sum((a - b) ** 2 for a, b in zip(c1[:3], c2[:3])))

def remove_alien_bg():
    img = Image.open('Content/alien1.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    # Start with the top-left corner color as the "background" reference
    bg_ref = pixels[0, 0]
    threshold = 40 # Slightly higher tolerance
    
    # Flood fill from corners
    queue = [(0, 0), (width-1, 0), (0, height-1), (width-1, height-1)]
    visited = set(queue)
    
    dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)]
    
    while queue:
        x, y = queue.pop(0)
        
        current_color = pixels[x, y]
        
        # If this pixel is close enough to the background reference color
        if color_distance(current_color, bg_ref) < threshold:
            # Make it transparent
            pixels[x, y] = (0, 0, 0, 0)
            
            # Check neighbors
            for dx, dy in dirs:
                nx, ny = x + dx, y + dy
                if 0 <= nx < width and 0 <= ny < height:
                    if (nx, ny) not in visited:
                        visited.add((nx, ny))
                        queue.append((nx, ny))
                        
    img.save('Content/alien1.png')
    print("Processed Content/alien1.png with flood fill transparency.")

if __name__ == "__main__":
    remove_alien_bg()
