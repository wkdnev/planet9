from PIL import Image

def inspect_alien_artifacts():
    img = Image.open('Content/alien1.png')
    img = img.convert('RGBA')
    pixels = img.load()
    width, height = img.size
    
    print(f"Size: {width}x{height}")
    
    # Sample pixels to find the artifacts
    # The user screenshot shows a box around the alien.
    # Let's look at pixels that are NOT transparent but also NOT the main red alien color.
    
    artifact_colors = {}
    
    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            if a > 0: # Visible
                # Simple heuristic: if it's grey-ish, it's likely background/artifact
                # Aliens are usually colorful.
                if abs(r - g) < 20 and abs(g - b) < 20:
                    color = (r, g, b, a)
                    artifact_colors[color] = artifact_colors.get(color, 0) + 1

    print("Potential artifact colors (Grey-ish):")
    sorted_artifacts = sorted(artifact_colors.items(), key=lambda x: x[1], reverse=True)
    for color, count in sorted_artifacts[:10]:
        print(f"{color}: {count}")

if __name__ == "__main__":
    inspect_alien_artifacts()
