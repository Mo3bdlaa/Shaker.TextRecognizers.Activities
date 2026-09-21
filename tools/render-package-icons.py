#!/usr/bin/env python3
"""Render the package icon shown beside the package in Studio's Manage Packages.

The mark is lines of text with a single span picked out, which is what the suite does.
The per-activity icons are separate: each activity carries its own glyph, drawn inline
in the Design project. assets/icons/ holds the vector source for both.

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

SUITE_GLYPH = "text-span"
PACKAGE = "Shaker.TextRecognizers.Activities"


def glyph_body(name):
    """The drawing commands of a source glyph, without its <svg> wrapper or comments."""
    text = (GLYPHS / f"{name}.svg").read_text()
    inner = text.split(">", 1)[1].rsplit("</svg>", 1)[0]
    return re.sub(r"<!--.*?-->", "", inner, flags=re.S).strip()


def main():
    # The 24-unit glyph is scaled to 30 and centred in a 48-unit rounded tile.
    svg = f"""<svg xmlns="http://www.w3.org/2000/svg" width="{SIZE}" height="{SIZE}" viewBox="0 0 48 48">
  <rect x="0" y="0" width="48" height="48" rx="10" fill="{TINT}"/>
  <g transform="translate(9 9) scale(1.25)" fill="none" stroke="{INDIGO}"
     stroke-width="1.9" stroke-linecap="round" stroke-linejoin="round">
    {glyph_body(SUITE_GLYPH)}
  </g>
</svg>"""

    out = ROOT / "src" / PACKAGE / "icon.png"
    cairosvg.svg2png(
        bytestring=svg.encode(), write_to=str(out),
        output_width=SIZE, output_height=SIZE,
    )
    print(f"{out.relative_to(ROOT)}  <- assets/icons/{SUITE_GLYPH}.svg")


if __name__ == "__main__":
    main()
