from PIL import Image

def inspect_image():
    img = Image.open('Content/player_ship_non_move.png')
    img = img.convert('RGBA')
    pixels = img.load()
    
    # Check corners to guess background color
    corners = [
        (0, 0),
        (img.width - 1, 0),
        (0, img.height - 1),
        (img.width - 1, img.height - 1)
    ]
    
    print("Corner colors:")
    for x, y in corners:
        print(f"({x}, {y}): {pixels[x, y]}")

if __name__ == "__main__":
    inspect_image()
