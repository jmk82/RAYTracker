# RAYTracker
C#/.NET/WPF/MVVM -ohjelma RAY-pokeritilastojen tarkasteluun

Ohjelma RAY:lla pelattujen pokerisessioiden noutamiseen RAY:n palvelimelta sekä tulosten ja tilastojen tarkasteluun.

Toimintoja:
- RAY:lle kirjautumalla voi noutaa omat käteispelisessioiden/turnausten tiedot valitulta aikajaksolta (vaikka kaikki kerralla, vrt. RAY:n oman käyttöliittymän 30 päivän maksimi)
- voi myös hakea RAY:n käyttöliittymällä ja kopioida rivit tekstitiedostoon ja tuoda ne ohjelmaan (alkuperäinen toiminnallisuus)
- samaan aikaan pelattujen useiden pöytien automaattinen ryhmittely samaan pelikertaan
- päivä-/kuukausi-/vuosi-/pelityyppitilastot
- bb/100, €/h, €/pöytä/tunti, multitable ratio ym. tilastoja
- tuotujen käteissessioiden ja turnausten tallennus XML-tiedostoon ja automaattinen avaus käynnistettäessä
- käteispelien filtteröinti ajanjakson sekä valittujen pelityyppien perusteella

Puutteita/kesken/ei toteutettu:
- virheidenkäsittely/loggaus
- ei automaattisia testejä
- turnauksista saa vain yksinkertaisen raportin

<p><a href="https://www.youtube.com/watch?v=zFDXRh5QRas">Ohjelman esittelyvideo (5 min)</a></p>
<p><a href="https://www.youtube.com/watch?v=4ghVsb3OLUg">Ohjelman tarkempi esittely (15 min)</a></p>

Käytettyjä tekniikoita/menetelmiä yms.:
- C#/.NET
- Windows Presentation Framework (WPF)
- Model-View-ViewModel (MVVM) (ei code-behindia)
- MVVM Light Toolkit (SimpleIoc, RelayCommand, Messenger) (http://www.mvvmlight.net)
- LINQ
