from PIL import Image

def check_alien_bg():
    img = Image.open('Content/alien1.png')
    img = img.convert('RGBA')
    pixels = img.load()
    
    # Check corners
    corners = [(0,0), (img.width-1, 0), (0, img.height-1), (img.width-1, img.height-1)]
    print("Corner pixels:")
    for x,y in corners:
        print(f"({x},{y}): {pixels[x,y]}")

if __name__ == "__main__":
    check_alien_bg()
