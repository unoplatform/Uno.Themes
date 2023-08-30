﻿/*
Fonts preloading is done via javascript here to make sure they are loaded very early in the app's life cycle.
Current way to do it directly in the project until we manage to have a generic fonts management in Uno.
Related Uno issue: https://github.com/unoplatform/uno/issues/693

When adding fonts here, make sure to add them using a base64 data uri, otherwise
fonts loading are delayed, and text may get displayed incorrectly.
*/

(function () {
	async function loadFont(fontFamily, fontSource, fontWeight) {
		const font = new FontFace(fontFamily, fontSource);
		if (fontWeight) {
			font.weight = fontWeight;
		}

		/* Wait for font to be loaded */
		await font.load();

		/* Add font to document */
		document.fonts.add(font);
	}

	/* SF Pro font (for Cupertino) */
})();