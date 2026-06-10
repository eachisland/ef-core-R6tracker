const API = 'http://localhost:5000/api';

let token = localStorage.getItem('r6_token');
let currentUser = JSON.parse(localStorage.getItem('r6_user') || 'null');
let allPlayers = [];

function showPage(name) {
    document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
    document.getElementById('page-' + name).classList.add('active');

    if (name === 'players') loadPlayers();
    if (name === 'sessions') loadSessions();
    if (name === 'ranks') loadRanks();
    if (name === 'maps') loadMaps();
    if (name === 'home') loadHomeStats();
    if (name === 'admin') loadAdminContent();
}

function updateAuthUI() {
    const loggedIn = !!token;
    const isAdmin = currentUser && currentUser.isAdmin;

    document.getElementById('auth-btn').style.display = loggedIn ? 'none' : '';
    document.getElementById('logout-btn').style.display = loggedIn ? '' : 'none';
    document.getElementById('add-player-btn').style.display = isAdmin ? '' : 'none';
    document.getElementById('add-session-btn').style.display = isAdmin ? '' : 'none';
    document.getElementById('admin-nav').style.display = isAdmin ? '' : 'none';
    document.getElementById('add-map-btn').style.display = isAdmin ? '' : 'none';
    document.getElementById('user-info').textContent = currentUser ? currentUser.displayName : '';
}

function switchTab(tab, btn) {
    document.querySelectorAll('.auth-tabs button').forEach(b => b.classList.remove('active'));
    btn.classList.add('active');
    document.getElementById('login-form').style.display = tab === 'login' ? '' : 'none';
    document.getElementById('register-form').style.display = tab === 'register' ? '' : 'none';
}

async function doLogin() {
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    try {
        const res = await fetch(`${API}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });

        const data = await res.json();

        if (!res.ok) {
            showAlert('login-error', data.message || 'Login failed', false);
            return;
        }

        token = data.token;
        currentUser = {
            email,
            displayName: data.displayName,
            isAdmin: data.isAdmin,
            playerId: data.playerId
        };

        localStorage.setItem('r6_token', token);
        localStorage.setItem('r6_user', JSON.stringify(currentUser));

        updateAuthUI();
        showPage('players');
    } catch (e) {
        showAlert('login-error', 'Cannot connect to API.', false);
    }
}

async function doRegister() {
    const email = document.getElementById('reg-email').value;
    const password = document.getElementById('reg-password').value;
    const displayName = document.getElementById('reg-displayname').value;
    const country = document.getElementById('reg-country').value;

    try {
        const res = await fetch(`${API}/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password, displayName, country })
        });

        const data = await res.json();

        if (!res.ok) {
            showAlert('reg-error', data.message || 'Registration failed', false);
            return;
        }

        showAlert('reg-error', 'Registered! You can now login.', true);
    } catch (e) {
        showAlert('reg-error', 'Cannot connect to API.', false);
    }
}

function logout() {
    token = null;
    currentUser = null;
    localStorage.removeItem('r6_token');
    localStorage.removeItem('r6_user');
    updateAuthUI();
    showPage('home');
}

async function loadPlayers() {
    const el = document.getElementById('players-list');
    el.innerHTML = '<div class="loading">Loading...</div>';

    try {
        const res = await fetch(`${API}/players`);
        allPlayers = await res.json();
        renderPlayers(allPlayers);
    } catch (e) {
        el.innerHTML = '<div class="loading">Cannot connect to API.</div>';
    }
}

function renderPlayers(players) {
    const el = document.getElementById('players-list');

    if (!players.length) {
        el.innerHTML = '<div class="loading">No players found.</div>';
        return;
    }

    el.innerHTML = players.map(p => `
        <div class="card">
            <div class="card-name">${esc(p.playerName)}</div>
            <div class="card-info">${esc(p.platform)} | ${esc(p.country)} | ${esc(p.rankName)}</div>
            <div class="stats-row">
                <div class="stat-box">
                    <span class="val">${p.level}</span>
                    <span class="key">Level</span>
                </div>
                <div class="stat-box">
                    <span class="val">${p.matchesPlayed}</span>
                    <span class="key">Matches</span>
                </div>
                <div class="stat-box">
                    <span class="val ${p.killDeathRatio >= 1 ? 'kd-good' : 'kd-bad'}">${p.killDeathRatio.toFixed(2)}</span>
                    <span class="key">K/D</span>
                </div>
            </div>
           ${currentUser && currentUser.isAdmin ? `
<div class="card-actions">
    <button onclick="openEditPlayer('${p.id}')">Edit</button>
    <button class="danger" onclick="deletePlayer('${p.id}')">Delete</button>
</div>` : ''}
        </div>
    `).join('');
}

function filterPlayers() {
    const search = document.getElementById('search-input').value.toLowerCase();
    const platform = document.getElementById('platform-filter').value;
    const filtered = allPlayers.filter(p =>
        (!search || p.playerName.toLowerCase().includes(search)) &&
        (!platform || p.platform === platform)
    );
    renderPlayers(filtered);
}

function openAddPlayer() {
    document.getElementById('player-modal-title').textContent = 'Add Player';
    document.getElementById('edit-player-id').value = '';
    document.getElementById('f-name').value = '';
    document.getElementById('f-level').value = 1;
    document.getElementById('f-country').value = '';
    document.getElementById('f-matches').value = 0;
    document.getElementById('f-kills').value = 0;
    document.getElementById('f-deaths').value = 0;
    document.getElementById('player-error').innerHTML = '';
    openModal('player-modal');
}

function openEditPlayer(id) {
    const p = allPlayers.find(x => x.id === id);
    if (!p) return;
    document.getElementById('player-modal-title').textContent = 'Edit Player';
    document.getElementById('edit-player-id').value = id;
    document.getElementById('f-name').value = p.playerName;
    document.getElementById('f-platform').value = p.platform;
    document.getElementById('f-level').value = p.level;
    document.getElementById('f-country').value = p.country;
    document.getElementById('f-matches').value = p.matchesPlayed;
    document.getElementById('f-kills').value = p.kills;
    document.getElementById('f-deaths').value = p.deaths;
    document.getElementById('player-error').innerHTML = '';
    openModal('player-modal');
}

async function savePlayer() {
    const id = document.getElementById('edit-player-id').value;
    const dto = {
        playerName: document.getElementById('f-name').value,
        platform: document.getElementById('f-platform').value,
        level: +document.getElementById('f-level').value,
        country: document.getElementById('f-country').value,
        matchesPlayed: +document.getElementById('f-matches').value,
        kills: +document.getElementById('f-kills').value,
        deaths: +document.getElementById('f-deaths').value
    };

    const url = id ? `${API}/players/${id}` : `${API}/players`;
    const method = id ? 'PUT' : 'POST';

    try {
        const res = await fetch(url, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            const d = await res.json();
            showAlert('player-error', d.message || 'Error saving player', false);
            return;
        }

        closeModal('player-modal');
        loadPlayers();
    } catch (e) {
        showAlert('player-error', 'Connection error', false);
    }
}

async function deletePlayer(id) {
    if (!confirm('Delete this player?')) return;
    await fetch(`${API}/players/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });
    loadPlayers();
}

async function loadSessions() {
    const el = document.getElementById('sessions-list');
    el.innerHTML = '<div class="loading">Loading...</div>';

    try {
        const res = await fetch(`${API}/gamesessions`);
        const sessions = await res.json();

        if (!sessions.length) {
            el.innerHTML = '<div class="loading">No sessions found.</div>';
            return;
        }

        const isAdmin = currentUser && currentUser.isAdmin;

        el.innerHTML = `
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Player</th>
                        <th>Map</th>
                        <th>Kills</th>
                        <th>Deaths</th>
                        <th>Result</th>
                        ${isAdmin ? '<th>Action</th>' : ''}
                    </tr>
                </thead>
                <tbody>
                    ${sessions.map(s => `
                        <tr>
                            <td>${new Date(s.datePlayed).toLocaleDateString()}</td>
                            <td>${esc(s.playerName)}</td>
                            <td>${esc(s.mapName || '-')}</td>
                            <td>${s.kills}</td>
                            <td>${s.deaths}</td>
                            <td class="${s.result === 'Win' ? 'win' : s.result === 'Loss' ? 'loss' : ''}">${esc(s.result)}</td>
                            ${isAdmin ? `<td><button class="danger" onclick="deleteSession('${s.id}')">Delete</button></td>` : ''}
                        </tr>
                    `).join('')}
                </tbody>
            </table>`;
    } catch (e) {
        el.innerHTML = '<div class="loading">Cannot connect to API.</div>';
    }
}

async function openAddSession() {
    const [pRes, mRes] = await Promise.all([
        fetch(`${API}/players`),
        fetch(`${API}/maps`)
    ]);

    const players = await pRes.json();
    const maps = await mRes.json();

    document.getElementById('s-player').innerHTML =
        players.map(p => `<option value="${p.id}">${esc(p.playerName)} (${esc(p.platform)})</option>`).join('');

    document.getElementById('s-map').innerHTML =
        maps.map(m => `<option value="${m.id}">${esc(m.name)}</option>`).join('');

    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    document.getElementById('s-date').value = now.toISOString().slice(0, 16);

    openModal('session-modal');
}

async function saveSession() {
    const dto = {
        playerId: document.getElementById('s-player').value,
        mapId: +document.getElementById('s-map').value,
        result: document.getElementById('s-result').value,
        datePlayed: document.getElementById('s-date').value,
        kills: +document.getElementById('s-kills').value,
        deaths: +document.getElementById('s-deaths').value
    };

    try {
        const res = await fetch(`${API}/gamesessions`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            const d = await res.json();
            showAlert('session-error', d.message || 'Error', false);
            return;
        }

        closeModal('session-modal');
        loadSessions();
    } catch (e) {
        showAlert('session-error', 'Connection error', false);
    }
}

async function deleteSession(id) {
    if (!confirm('Delete this session?')) return;
    await fetch(`${API}/gamesessions/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });
    loadSessions();
}

async function loadRanks() {
    const el = document.getElementById('ranks-list');
    el.innerHTML = '<div class="loading">Loading...</div>';

    try {
        const res = await fetch(`${API}/ranks`);
        const ranks = await res.json();
        const icons = ['🟤', '🟤', '⬜', '🟡', '🔵', '💎', '👑'];

        el.innerHTML = ranks.map((r, i) => `
            <div class="card" style="text-align:center; padding:2rem">
                <div style="font-size:2rem; margin-bottom:0.5rem">${icons[i] || '🎖️'}</div>
                <div class="card-name">${esc(r.name)}</div>
                <div class="card-info">Tier ${r.tier}</div>
            </div>
        `).join('');
    } catch (e) {
        el.innerHTML = '<div class="loading">Cannot connect to API.</div>';
    }
}

async function loadMaps() {
    const el = document.getElementById('maps-list');
    el.innerHTML = '<div class="loading">Loading...</div>';

    try {
        const res = await fetch(`${API}/maps`);
        const maps = await res.json();
        const isAdmin = currentUser && currentUser.isAdmin;

        el.innerHTML = maps.map(m => `
            <div class="card">
                <div class="card-name">${esc(m.name)}</div>
                <div class="card-info">${esc(m.location)} | ${esc(m.type)} | ${m.isActive ? 'Active' : 'Inactive'}</div>
                ${isAdmin ? `
                <div class="card-actions">
                    <button class="danger" onclick="deleteMap(${m.id})">Delete</button>
                </div>` : ''}
            </div>
        `).join('');
    } catch (e) {
        el.innerHTML = '<div class="loading">Cannot connect to API.</div>';
    }
}

function openAddMap() {
    document.getElementById('m-name').value = '';
    document.getElementById('m-location').value = '';
    document.getElementById('map-error').innerHTML = '';
    openModal('map-modal');
}

async function saveMap() {
    const dto = {
        name: document.getElementById('m-name').value,
        location: document.getElementById('m-location').value,
        type: document.getElementById('m-type').value,
        isActive: document.getElementById('m-active').value === 'true'
    };

    try {
        const res = await fetch(`${API}/maps`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            const d = await res.json();
            showAlert('map-error', d.message || 'Error', false);
            return;
        }

        closeModal('map-modal');
        loadMaps();
    } catch (e) {
        showAlert('map-error', 'Connection error', false);
    }
}

async function deleteMap(id) {
    if (!confirm('Delete this map?')) return;
    await fetch(`${API}/maps/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });
    loadMaps();
}

async function loadHomeStats() {
    try {
        const [pRes, sRes, mRes] = await Promise.all([
            fetch(`${API}/players`),
            fetch(`${API}/gamesessions`),
            fetch(`${API}/maps`)
        ]);

        document.getElementById('stat-players').textContent = (await pRes.json()).length;
        document.getElementById('stat-sessions').textContent = (await sRes.json()).length;
        document.getElementById('stat-maps').textContent = (await mRes.json()).length;
    } catch (e) { }
}

async function loadAdminContent() {
    const el = document.getElementById('admin-content');
    el.innerHTML = '<div class="loading">Loading...</div>';

    try {
        const res = await fetch(`${API}/players`);
        const players = await res.json();

        el.innerHTML = `
            <h3 style="margin-bottom:1rem; color:white">All Players (${players.length})</h3>
            <div class="grid">
                ${players.map(p => `
                    <div class="card">
                        <div class="card-name">${esc(p.playerName)}</div>
                        <div class="card-info">${esc(p.platform)} | ${esc(p.country)}</div>
                        <div class="card-actions">
                            <button class="danger" onclick="deletePlayer('${p.id}'); loadAdminContent()">Delete</button>
                        </div>
                    </div>
                `).join('')}
            </div>`;
    } catch (e) {
        el.innerHTML = '<div class="loading">Cannot connect to API.</div>';
    }
}

function openModal(id) {
    document.getElementById(id).classList.add('open');
}

function closeModal(id) {
    document.getElementById(id).classList.remove('open');
}

function showAlert(containerId, msg, isSuccess) {
    document.getElementById(containerId).innerHTML =
        `<div class="alert ${isSuccess ? 'alert-success' : 'alert-error'}">${msg}</div>`;
}

function esc(s) {
    return String(s)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

document.querySelectorAll('.modal-overlay').forEach(o => {
    o.addEventListener('click', e => {
        if (e.target === o) o.classList.remove('open');
    });
});

updateAuthUI();
loadHomeStats();