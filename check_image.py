from PIL import Image
import os

try:
    img = Image.open('Content/player_ship.png')
    print(f"Image found. Dimensions: {img.size}")
    print(f"Format: {img.format}")
    print(f"Mode: {img.mode}")
except Exception as e:
    print(f"Error reading image: {e}")
