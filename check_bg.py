from PIL import Image

def check_bg_size():
    try:
        img = Image.open('Content/level1_bg.png')
        print(f"Background size: {img.size}")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    check_bg_size()
