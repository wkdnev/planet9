from PIL import Image

def inspect_artifacts():
    img = Image.open('Content/player_ship_non_move.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    print(f"Size: {width}x{height}")
    
    # Sample a grid of pixels to see what's around the ship
    print("Sampling pixels:")
    for y in range(0, height, 5):
        row = []
        for x in range(0, width, 5):
            p = pixels[x, y]
            # formatting for brevity: R,G,B,A
            row.append(f"{p[0]},{p[1]},{p[2]},{p[3]}")
        print(f"Row {y}: " + " | ".join(row))

if __name__ == "__main__":
    inspect_artifacts()
