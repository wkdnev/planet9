from PIL import Image

def analyze_good_alien():
    try:
        img = Image.open('Content/alien_good.png')
        print(f"Size: {img.size}")
        print(f"Mode: {img.mode}")
        
        # Check corners for background
        img = img.convert('RGBA')
        pixels = img.load()
        corners = [(0,0), (img.width-1, 0), (0, img.height-1), (img.width-1, img.height-1)]
        print("Corner pixels:")
        for x,y in corners:
            print(f"({x},{y}): {pixels[x,y]}")
            
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    analyze_good_alien()
