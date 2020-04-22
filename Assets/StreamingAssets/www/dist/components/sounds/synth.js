export class Synth {
  constructor(){
    let iframe = document.createElement("iframe");
    iframe.style.display = "none";
    document.body.appendChild(iframe);
    iframe.src = "/assets/sounds/synth/pianosynth.html";
    this.sy = iframe.contentWindow;
  }
  noteOn(note, velo) {
    this.sendMessage(this.sy, "midi,90," + note.toString(16) + "," + velo.toString(16));
  }
  noteOff(note) {
    this.sendMessage(this.sy, "midi,80," + note.toString(16) + ",0");
  }
  allSoundOff() {
    this.sendMessage(this.sy, "midi,b0,78,0");
  }
  sendMessage(sy, s) {
    if(this.sy)
      this.sy.postMessage(s, "*");
  }
}
