#include <stdarg.h>
#include <stdio.h>
#ifdef __APPLE__
#include <util.h>
#else // __APPLE__
#include <pty.h>
#include <termios.h>
#endif // __APPLE__
#include <sys/ioctl.h>
#include <sys/types.h>
#include <sys/uio.h>
#include <unistd.h>
#include <sys/select.h>
#include <sys/wait.h>
#include <stdlib.h>

int pty_init(int *master_fd, unsigned short column, unsigned short row)
{
    pid_t pid;
    struct winsize ws;
    struct termios term;

    ws.ws_col = column;
    ws.ws_row = row;
    ws.ws_xpixel = 0;
    ws.ws_ypixel = 0;

    term.c_iflag = ICRNL | IXON | IXANY | IMAXBEL | BRKINT | IUTF8;
    term.c_oflag = OPOST | ONLCR;
    term.c_cflag = CREAD | CS8 | HUPCL;
    term.c_lflag = ICANON | ISIG | IEXTEN | ECHO | ECHOE | ECHOK | ECHOKE | ECHOCTL;
    term.c_ospeed = B38400;
    term.c_cc[VEOF] = 4;
    term.c_cc[VEOL] = -1;
    term.c_cc[VEOL2] = -1;
    term.c_cc[VERASE] = 0x7f;
    term.c_cc[VWERASE] = 23;
    term.c_cc[VKILL] = 21;
    term.c_cc[VREPRINT] = 18;
    term.c_cc[VINTR] = 3;
    term.c_cc[VQUIT] = 0x1c;
    term.c_cc[VSUSP] = 26;
    term.c_cc[VSTART] = 17;
    term.c_cc[VSTOP] = 19;
    term.c_cc[VLNEXT] = 22;
    term.c_cc[VDISCARD] = 15;
    term.c_cc[VMIN] = 1;
    term.c_cc[VTIME] = 0;
#ifdef __APPLE__
    term.c_cc[VDSUSP] = 25;
    term.c_cc[VSTATUS] = 20;
#endif // __APPLE__

    pid = forkpty(master_fd, NULL, &term, &ws);

    return pid;
}

int pty_peek(int fd)
{
    fd_set rfds;
    struct timeval tv;
    int retval;

    FD_ZERO(&rfds);
    FD_SET(fd, &rfds);

    tv.tv_sec = 0;
    tv.tv_usec = 0;

    return select(fd + 1, &rfds, NULL, NULL, &tv);
}

int pty_read(int fd, char *buffer, int size)
{
    return read(fd, buffer, size);
}

int pty_write(int fd, char *buffer, int size)
{
    return write(fd, buffer, size);
}

int pty_resize(int fd, unsigned short column, unsigned short row)
{
    struct winsize ws;

    ws.ws_col = column;
    ws.ws_row = row;
    ws.ws_xpixel = 0;
    ws.ws_ypixel = 0;

    return ioctl(fd, TIOCSWINSZ, &ws);
}

int pty_waitpid(int pid, int *status, int options)
{
    return waitpid(pid, status, options);
}

int pty_close(int fd)
{
    return close(fd);
}

int pty_execvp(const char *file, char *const argv[])
{
    return execvp(file, argv);
}

int pty_setenv(const char *name, const char *value, int overwrite)
{
    return setenv(name, value, overwrite);
}