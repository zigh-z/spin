document.addEventListener('DOMContentLoaded', () => {
  const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');
  const elements = document.querySelectorAll('.section-kicker, .section h2, .section-intro, .benefits article, .layers article, .steps li, .calc-grid article, .total-card, .water-demo');
  if ('IntersectionObserver' in window && !reducedMotion.matches) {
    const observer = new IntersectionObserver(entries => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.classList.add('is-visible');
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.08 });
    elements.forEach(element => { element.classList.add('reveal'); observer.observe(element); });
    document.body.classList.add('motion-ready');
  }
  const progress = document.querySelector('.reading-progress');
  const links = document.querySelectorAll('.nav-links a');
  const sections = [...links].map(link => document.querySelector(link.getAttribute('href')));
  let scheduled = false;
  const updateScroll = () => {
    const range = document.documentElement.scrollHeight - window.innerHeight;
    progress.style.transform = `scaleX(${range > 0 ? Math.min(1, Math.max(0, window.scrollY / range)) : 0})`;
    let active = -1;
    sections.forEach((section, index) => { if (section.getBoundingClientRect().top <= 160) active = index; });
    links.forEach((link, index) => { if (index === active) link.setAttribute('aria-current', 'location'); else link.removeAttribute('aria-current'); });
    scheduled = false;
  };
  window.addEventListener('scroll', () => { if (!scheduled) { scheduled = true; requestAnimationFrame(updateScroll); } }, { passive: true });
  window.addEventListener('resize', updateScroll);
  updateScroll();
  const video = document.querySelector('#spin-video');
  if (location.protocol === 'file:' && window.spinCaptions) {
    const captionUrl = URL.createObjectURL(new Blob([window.spinCaptions], { type: 'text/vtt' }));
    video.querySelector('track').src = captionUrl;
    window.addEventListener('pagehide', () => URL.revokeObjectURL(captionUrl), { once: true });
  }
  const chapters = window.spinChapters || [];
  const chapterList = document.querySelector('.chapter-links');
  const transcript = document.querySelector('#video-transcript');
  chapters.forEach((chapter, index) => {
    const heading = document.createElement('h3');
    heading.textContent = chapter.title;
    const text = document.createElement('p');
    text.textContent = chapter.text;
    transcript.append(heading, text);
    if (index > 0 && index < chapters.length - 1) {
      const button = document.createElement('button');
      button.type = 'button';
      button.textContent = chapter.title;
      button.addEventListener('click', () => {
        const seek = () => { video.currentTime = chapter.start; video.play().catch(() => {}); };
        if (video.readyState >= 1) seek();
        else { video.addEventListener('loadedmetadata', seek, { once: true }); video.load(); }
      });
      chapterList.append(button);
    }
  });
  video.addEventListener('timeupdate', () => {
    [...chapterList.children].forEach((button, index) => {
      const chapter = chapters[index + 1];
      const active = video.currentTime >= chapter.start && video.currentTime < chapter.end;
      button.classList.toggle('chapter-active', active);
      if (active) button.setAttribute('aria-current', 'true');
      else button.removeAttribute('aria-current');
    });
  });
  document.querySelectorAll('.diagram img').forEach(img => { img.loading = 'lazy'; img.decoding = 'async'; });
});
