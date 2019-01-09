Builds navigiation as an HTML unordered list using JSON data

 [
  {
    "title": "About",
    "link": "/about/",
    "childPages": [
      {
        "title": "About Sub 1",
        "link": "/about/AboutSub1"
      },
      {
        "title": "About Sub 2",
        "link": "/about/AboutSub2",
        "childPages": [
          {
            "title": "About Sub 2 Child",
            "link":  "/about/AboutSub2/AboutSub2Child"
          }
        ]
      }
    ]
  },
  {
		"title": "Contact",
		"link": "/contact/",
		"childPages": [
			{
				"title": "Contact Sub 1",
				"link": "/contact/ContactSub1"
			},
			{
				"title": "Contact Sub 2",
				"link": "/contact/ContactSub2"
			}
		]
	}
]
