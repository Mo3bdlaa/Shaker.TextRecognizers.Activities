#!/usr/bin/env python3
"""Render one package icon per activity package from the source glyphs.

Each package shows the same glyph its activities carry in Studio's panel, so a
package and its activities read as one set. The glyphs in assets/icons/*.svg are the
single source: edit one, re-run this, and the package icon refreshes.

    pip install cairosvg
    python tools/render-package-icons.py
"""

import pathlib
import re

import cairosvg

ROOT = pathlib.Path(__file__).resolve().parent.parent
GLYPHS = ROOT / "assets" / "icons"

# The same indigo the panel icons are drawn in, on a soft wash of it. Keeping the
# glyph well inside the tile leaves it legible where Studio draws the icon small.
INDIGO = "#5B5BD6"
TINT = "#EEF0FD"
SIZE = 128

# package -> the glyph it shares with its activities
PACKAGES = {
    "TextRecognizers.DateTime.Activities": "calendar",
    "TextRecognizers.Number.Activities": "hash",
    "TextRecognizers.NumberWithUnit.Activities": "ruler",
    "TextRecognizers.Sequence.Activities": "envelope",
    "TextRecognizers.Choice.Activities": "toggle",
}


def glyph_body(name):
    """The drawing commands of a source glyph, without its <svg> wrapper or comments."""
    text = (GLYPHS / f"{name}.svg").read_text()
    inner = text.split(">", 1)[1].rsplit("</svg>", 1)[0]
    return re.sub(r"<!--.*?-->", "", inner, flags=re.S).strip()


def main():
    for package, glyph in PACKAGES.items():
        # The 24-unit glyph is scaled to 30 and centred in a 48-unit rounded tile.
        svg = f"""<svg xmlns="http://www.w3.org/2000/svg" width="{SIZE}" height="{SIZE}" viewBox="0 0 48 48">
  <rect x="0" y="0" width="48" height="48" rx="10" fill="{TINT}"/>
  <g transform="translate(9 9) scale(1.25)" fill="none" stroke="{INDIGO}"
     stroke-width="1.9" stroke-linecap="round" stroke-linejoin="round">
    {glyph_body(glyph)}
  </g>
</svg>"""

        out = ROOT / "src" / package / "icon.png"
        cairosvg.svg2png(
            bytestring=svg.encode(), write_to=str(out),
            output_width=SIZE, output_height=SIZE,
        )
        print(f"{out.relative_to(ROOT)}  <- assets/icons/{glyph}.svg")


if __name__ == "__main__":
    main()
