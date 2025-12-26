from PIL import Image, ImageDraw

def create_spaceship():
    # Create a 32x32 image with transparent background
    size = (32, 32)
    img = Image.new('RGBA', size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Colors
    body_color = (50, 150, 255, 255)  # Blueish
    cockpit_color = (200, 200, 255, 255) # Light blue
    engine_color = (255, 100, 50, 255) # Orange
    outline_color = (200, 200, 200, 255) # White/Grey

    # Draw main body (triangle-ish)
    # Points: Top-Center, Bottom-Left, Bottom-Right
    points = [
        (16, 2),   # Nose
        (4, 28),   # Left Wing
        (28, 28),  # Right Wing
        (16, 24)   # Bottom Center indent
    ]
    draw.polygon(points, fill=body_color, outline=outline_color)

    # Draw Cockpit
    draw.ellipse([14, 10, 18, 18], fill=cockpit_color)

    # Draw Engines
    draw.rectangle([10, 28, 14, 31], fill=engine_color)
    draw.rectangle([18, 28, 22, 31], fill=engine_color)
    
    # Save
    img.save('Content/player_ship.png')
    print("Created Content/player_ship.png")

if __name__ == "__main__":
    create_spaceship()
