# Animação elaborada do telhado verde

Arquivo publicado: `telhado-verde-espin/assets/telhado-verde-spin-vivo.mp4`.

Esta versão renderiza cada capítulo inteiro, com duração igual à narração, sem repetir ciclos curtos. A abertura aproveita a aproximação da escola no vídeo fornecido pelo usuário.

Em cada explicação, a câmera muda de ângulo, a cobertura se abre, a camada selecionada ocupa o quadro e depois volta ao conjunto. As funções recebem ações próprias: crescimento das plantas e raízes, filtragem de partículas, escoamento, proteção contra raízes, impermeabilização, isolamento e suporte estrutural. As legendas e os nove atalhos por camada continuam disponíveis.

O corte é uma ilustração didática em perspectiva, sem escala, e não uma simulação física.

Para reproduzir no Windows, na raiz do projeto:

    powershell -NoProfile -ExecutionPolicy Bypass -File tools/create-living.ps1
    powershell -NoProfile -ExecutionPolicy Bypass -File tools/assemble-living.ps1

Dependências locais: FFmpeg, System.Drawing, tools/reference-spin.mp4, assets/escola-spin.png, assets/voice-*.wav, tools/scenes.json e assets/video-chapters.js.
