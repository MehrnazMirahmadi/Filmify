document.addEventListener('DOMContentLoaded', () => {
    const searchBtn = document.getElementById('filmSearchBtn');
    const searchInput = document.getElementById('filmSearchInput');
    const resultsContainer = document.getElementById('filmResultsContainer');

    let lastKey = 0; 
    let currentSearch = '';
    let isLoading = false;

    const isGetAllPage = window.location.pathname.includes('/Films/GetAll');
    const isHomePage = window.location.pathname === '/' || window.location.pathname.includes('/Home');

    async function fetchResults() {
        if (isLoading) return;
        isLoading = true;
        try {
            console.log('fetching with lastKey=', lastKey, 'search=', currentSearch);
            const res = await fetch(`/Films/Search?searchText=${encodeURIComponent(currentSearch)}&lastKey=${lastKey}`);
            const html = await res.text();

          
            if (lastKey === 0) resultsContainer.innerHTML = '';

           
            resultsContainer.querySelectorAll('#loadMoreBtn').forEach(btn => btn.remove());

            
            resultsContainer.insertAdjacentHTML('beforeend', html);

            const nextBtn = document.getElementById('loadMoreBtn');
            console.log('got loadMoreBtn?', !!nextBtn, nextBtn ? nextBtn.dataset.lastKey : null);
           
        } catch (err) {
            console.error('fetchResults error:', err);
        } finally {
            isLoading = false;
        }
    }


    resultsContainer.addEventListener('click', (e) => {
        const btn = e.target.closest('#loadMoreBtn');
        if (!btn) return;
      
        lastKey = Number(btn.dataset.lastKey) || 0;
        fetchResults();
    });

    function redirectToGetAll() {
        if (currentSearch.trim()) {
            window.location.href = `/Films/GetAll?searchText=${encodeURIComponent(currentSearch)}`;
        } else {
            window.location.href = '/Films/GetAll';
        }
    }

    searchBtn.addEventListener('click', () => {
        currentSearch = searchInput.value.trim();
        lastKey = 0;
        if (currentSearch === '') {
            if (isGetAllPage) redirectToGetAll();
            else fetchResults();
        } else {
            fetchResults();
        }
    });

    searchInput.addEventListener('keyup', (e) => {
        currentSearch = e.target.value.trim();
        lastKey = 0;
        if (currentSearch === '') {
            if (isGetAllPage) redirectToGetAll();
            else fetchResults();
        } else {
            fetchResults();
        }
    });

 
    const initialBtn = document.getElementById('loadMoreBtn');
    if (initialBtn) {
        console.log('initial loadMoreBtn present, dataset.lastKey=', initialBtn.dataset.lastKey);
        
    }
});
