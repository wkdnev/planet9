from PIL import Image

def find_green_eyes():
    img = Image.open('Content/alien_good.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    green_pixels = []
    
    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            # Check for green: G is significantly larger than R and B
            if g > r + 20 and g > b + 20:
                green_pixels.append((x, y, (r, g, b)))
                
    print(f"Found {len(green_pixels)} green pixels.")
    if len(green_pixels) > 0:
        print(f"Sample green pixel: {green_pixels[0]}")
        # Calculate bounding box of green pixels
        xs = [p[0] for p in green_pixels]
        ys = [p[1] for p in green_pixels]
        print(f"Green bounds: ({min(xs)}, {min(ys)}) to ({max(xs)}, {max(ys)})")

if __name__ == "__main__":
    find_green_eyes()
