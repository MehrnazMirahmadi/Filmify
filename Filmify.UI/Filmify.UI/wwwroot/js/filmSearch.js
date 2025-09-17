// filmSearch.js

export class FilmSearch {
    constructor(inputId, resultsId, loadMoreId, apiBaseUrl, pageSize = 10) {
        this.searchInput = document.getElementById(inputId);
        this.resultsDiv = document.getElementById(resultsId);
        this.loadMoreBtn = document.getElementById(loadMoreId);

        this.apiBaseUrl = apiBaseUrl;
        this.pageSize = pageSize;

        this.lastKey = null;
        this.currentSearch = '';
        this.debounceTimer = null;

        this.init();
    }

    init() {
        // Live search
        this.searchInput.addEventListener('keyup', (e) => {
            this.currentSearch = e.target.value;
            this.lastKey = null;
            clearTimeout(this.debounceTimer);
            this.debounceTimer = setTimeout(() => this.fetchFilms(), 300);
        });

        // Load More
        this.loadMoreBtn.addEventListener('click', () => this.fetchFilms());
    }

    async fetchFilms() {
        const url = `${this.apiBaseUrl}/api/films/search?searchText=${encodeURIComponent(this.currentSearch)}&pageSize=${this.pageSize}&lastKey=${this.lastKey || ''}`;
        try {
            const res = await fetch(url);
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            const data = await res.json();
            this.renderFilms(data);
        } catch (err) {
            console.error(err);
        }
    }

    renderFilms(data) {
        if (!data || !data.items) return;

        if (!this.lastKey) this.resultsDiv.innerHTML = '';

        data.items.forEach(film => {
            const filmDiv = document.createElement('div');
            filmDiv.classList.add('film-item');
            filmDiv.innerHTML = `
                <img src="/imgs/covers/${film.coverImage}" alt="${film.filmTitle}" />
                <h3>${film.filmTitle}</h3>
                <p>Category: ${film.categoryName}</p>
                <p>Tags: ${film.tags ? film.tags.join(', ') : ''}</p>
            `;
            this.resultsDiv.appendChild(filmDiv);
        });

        if (data.hasNextPage) {
            this.lastKey = data.lastKey;
            this.loadMoreBtn.style.display = 'block';
        } else {
            this.lastKey = null;
            this.loadMoreBtn.style.display = 'none';
        }
    }
}
