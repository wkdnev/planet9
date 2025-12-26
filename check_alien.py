from PIL import Image

def check_alien_size():
    try:
        img = Image.open('Content/alien1.png')
        print(f"Alien size: {img.size}")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    check_alien_size()
