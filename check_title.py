from PIL import Image

def check_title_size():
    try:
        img = Image.open('Content/title_screen.png')
        print(f"Title screen size: {img.size}")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    check_title_size()
